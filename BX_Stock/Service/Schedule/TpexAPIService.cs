using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    /// <summary>
    /// 上櫃個股 Service
    /// </summary>
    public class TpexAPIService : ITpexAPIService
    {
        /// <summary>
        /// 基礎 APIService
        /// </summary>
        private readonly IBaseApiService BaseApiService;

        /// <summary>
        /// DbContext
        /// </summary>
        private readonly StockContext StockContext;

        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper Mapper;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="baseApiService">基礎APIService</param>
        /// <param name="stockContext">DbContext</param>
        /// <param name="mapper">Mapper</param>
        public TpexAPIService(
            IBaseApiService baseApiService,
            StockContext stockContext,
            IMapper mapper
            )
        {
            this.BaseApiService = baseApiService;
            this.Mapper = mapper;
            this.StockContext = stockContext;
        }

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockScheduleFirst(int start, int end)
        {
            List<int> currentDbStockNo = this.StockContext.Set<Stock>().Where(w => !w.IsListed).Select(s => s.StockNo).ToList();
            currentDbStockNo = currentDbStockNo.Where(w => start <= w && w < end).ToList();

            // 撈歷史資料
            currentDbStockNo.ForEach(x => this.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        /// <summary>
        /// 新增上櫃個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        /// <param name="endMonth">查詢結束時間</param>
        public void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01", string endMonth = "2019-10")
        {
            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            DateTime currentMonth = string.IsNullOrEmpty(endMonth) ? DateTime.Now : DateTime.Parse(endMonth);
            List<IStockEntity> result = new List<IStockEntity>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(currentMonth))
            {
                (StockDayDto stockDayDto, int recordCount) = RetryHelper.RetryIfThrown<Exception, (StockDayDto, int)>(() =>
                                                                    this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult(), 3, 3);

                if (recordCount < 1)
                {
                    continue;
                }

                result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            result.ForEach(f => f.StockNo = stockNo);

            // 計算KD
            //result.CalcKD();

            this.StockContext.AddRange(result);
            this.StockContext.SaveChanges();
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        private async Task<(StockDayDto, int)> GetStockDataAsync(int stockNo, DateTime date)
        {
            TpexStockDayRequestParamDto test = new TpexStockDayRequestParamDto()
            {
                D = date.ConvertToTaiwanType(),  // "108/09",
                Stkno = stockNo.ToString()
            };

            // 連續串證交所API 會被鎖IP，每隔三秒串一次
            //System.Threading.Thread.Sleep(3000);

            TpexStockDayResponseDto stockData = await this.BaseApiService
                .GetAsync<TpexStockDayResponseDto, TpexStockDayRequestParamDto>(StockApiUrl.TpexStockDay, test);

            StockDayDetailDto[] detailDto = new StockDayDetailDto[stockData.ITotalRecords];

            // mapping
            for (int i = 0; i < stockData.ITotalRecords; i++)
            {
                detailDto[i] = new StockDayDetailDto
                {
                    Date = DateTime.Parse(stockData.AaData[i][0].ConvertToADType().Substring(0, 10)),
                    TradeVolume = Convert.ToInt32(stockData.AaData[i][1].Replace(",", string.Empty)),
                    TradeValue = Convert.ToDouble(stockData.AaData[i][2].CheckDoubleValue()),
                    OpeningPrice = Convert.ToDouble(stockData.AaData[i][3].CheckDoubleValue()),
                    HighestPrice = Convert.ToDouble(stockData.AaData[i][4].CheckDoubleValue()),
                    LowestPrice = Convert.ToDouble(stockData.AaData[i][5].CheckDoubleValue()),
                    ClosingPrice = Convert.ToDouble(stockData.AaData[i][6].CheckDoubleValue()),
                    Change = Convert.ToDouble(stockData.AaData[i][7].CheckDoubleValue()),
                    Transaction = Convert.ToInt32(stockData.AaData[i][8].Replace(",", string.Empty))
                };
            }

            StockDayDto result = new StockDayDto()
            {
                IsOK = stockData.StkNo.Equals(stockNo),
                Date = stockData.ReportDate.ConvertToADType(),
                StockNo = stockData.StkNo,
                Data = detailDto
            };

            return (result, stockData.ITotalRecords);
        }
    }
}