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
    /// 上市個股Service
    /// </summary>
    public class TwseAPIService : ITwseAPIService
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
        /// AutoMapper
        /// </summary>
        private readonly IMapper Mapper;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="baseApiService">基礎APIService</param>
        /// <param name="stockContext">DbContext</param>
        /// <param name="mapper">AutoMapper</param>
        public TwseAPIService(
            IBaseApiService baseApiService,
            StockContext stockContext,
            IMapper mapper)
        {
            this.BaseApiService = baseApiService;
            this.StockContext = stockContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockScheduleFirst(int start, int end)
        {
            List<int> currentDbStockNo = this.StockContext.Set<Stock>().Where(w => w.IsListed).Select(s => s.StockNo).ToList();
            currentDbStockNo = currentDbStockNo.Where(w => start <= w && w < end).ToList();

            // 撈11月份歷史資料
            currentDbStockNo.ForEach(x => this.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        /// <summary>
        /// 新增上市個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        /// <param name="endMonth">查詢結束時間</param>
        public void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01-04", string endMonth = "2019-10")
        {
            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            DateTime currentMonth = string.IsNullOrEmpty(endMonth) ? DateTime.Now : DateTime.Parse(endMonth);
            List<IStockEntity> result = new List<IStockEntity>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(currentMonth))
            {
                (StockDayDto stockDayDto, string stat) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

                if (stat.Contains("沒有符合條件"))
                {
                    continue;
                }

                result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            result.ForEach(f => f.StockNo = stockNo);

            // 計算KD
            result.CalcKD();

            this.StockContext.AddRange(result);
            this.StockContext.SaveChanges();
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        private async Task<(StockDayDto, string)> GetStockDataAsync(int stockNo, DateTime date)
        {
            TwseStockDayRequestParamDto requestParam = new TwseStockDayRequestParamDto()
            {
                Date = date.ToString("yyyyMMdd"),  //"20190901",
                StockNo = stockNo.ToString()
            };

            // 連續串證交所API 會被鎖IP，每隔三秒串一次
            System.Threading.Thread.Sleep(3000);

            TwseStockDayResponseDto stockData = await this.BaseApiService
                .GetAsync<TwseStockDayResponseDto, TwseStockDayRequestParamDto>(StockApiUrl.TwseStockDay, requestParam);

            StockDayDto result = this.Mapper.Map<StockDayDto>(stockData);

            return (result, stockData.Stat);
        }
    }
}