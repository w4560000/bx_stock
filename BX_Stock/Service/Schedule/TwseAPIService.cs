using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto.TwseDto;
using BX_Stock.Models.Entity;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public class TwseAPIService : ITwseAPIService
    {
        private readonly IBaseApiService BaseApiService;

        private readonly IWebCrawlerService WebCrawlerService;

        private readonly IStockService StockService;

        private readonly StockContext StockContext;

        private readonly IMapper Mapper;

        public TwseAPIService(
            IBaseApiService baseApiService,
            IWebCrawlerService webCrawlerService,
            IStockService stockService,
            StockContext stockContext,
            IMapper mapper)
        {
            this.BaseApiService = baseApiService;
            this.WebCrawlerService = webCrawlerService;
            this.StockService = stockService;
            this.StockContext = stockContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// 每日排程 撈取現有股號 (Schedule1)
        /// 若撈取的資料與現有資料庫股號有差異
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockSchedule1()
        {
            List<Stock> allStockData = new List<Stock>();

            // 爬取上市股票
            allStockData.AddRange(this.WebCrawlerService.GetAllListedStockNoAsync().GetAwaiter().GetResult());

            // 爬取上櫃股票
            allStockData.AddRange(this.WebCrawlerService.GetAllCabinetStockNoAsync().GetAwaiter().GetResult());

            List<Stock> currentDbStockData = this.StockContext.Set<Stock>().ToList();

            List<string> allStockNo = allStockData.Select(s => s.StockNo).ToList();
            List<string> currentDbStockNo = currentDbStockData.Select(s => s.StockNo).ToList();

            // 目前DB沒有的個股，但市面上有的(新上市)，新增該股資料
            List<string> insertStockNoList = allStockNo.Except(currentDbStockNo).ToList();
            this.InsertStock(allStockData.Where(x => insertStockNoList.Contains(x.StockNo)).ToList());

            //// 目前DB有的個股，但市面上沒有的(下市)，刪除該股在DB的資料
            List<string> deleteStockNoList = currentDbStockNo.Except(allStockNo).ToList();
            this.StockService.DeleteStockData(deleteStockNoList);
        }

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockScheduleFirst(int start, int end)
        {
            List<string> currentDbStockNo = this.StockContext.Set<Stock>().Select(s => s.StockNo).ToList();

            List<int> insertStockNo = new List<int>();
            currentDbStockNo.ForEach(x => insertStockNo.Add(Convert.ToInt32(x)));
            insertStockNo = insertStockNo.Where(w => start <= w && w < end).ToList();
            currentDbStockNo.Clear();
            insertStockNo.ForEach(x => currentDbStockNo.Add(x.ToString()));

            // 撈11月份歷史資料
            currentDbStockNo.ForEach(x => this.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        /// <summary>
        /// 新增個股資訊
        /// </summary>
        /// <param name="stockNoList">要新增的個股代號</param>
        private void InsertStock(List<Stock> insertStockList)
        {
            List<string> insertStockNoList = insertStockList.Select(s => s.StockNo).ToList();

            // 新增個股代號
            this.StockContext.BulkInsert(insertStockList);

            // 撈歷史資料
            insertStockNoList.ForEach(x => this.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            insertStockNoList.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            insertStockNoList.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        /// <summary>
        /// 新增個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        /// <param name="endMonth">查詢結束時間</param>
        private void ProcessStockHistoryData(string stockNo, string startMonth = "2010-01-04", string endMonth = "2019-10")
        {
            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            DateTime currentMonth = string.IsNullOrEmpty(endMonth) ? DateTime.Now : DateTime.Parse(endMonth);
            List<IStockEntity> result = new List<IStockEntity>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(currentMonth))
            {
                (StockDayDto stockDayDto, string stat) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

                if(stat.Contains("沒有符合條件"))
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
        private async Task<(StockDayDto, string)> GetStockDataAsync(string stockNo, DateTime date)
        {
            StockDayRequestParamDto test = new StockDayRequestParamDto()
            {
                Date = date.ToString("yyyyMMdd"),  //"20190901",
                StockNo = stockNo
            };

            // 連續串證交所API 會被鎖IP，每隔三秒串一次
            System.Threading.Thread.Sleep(3000);

            StockDayResponseDto stockData = await this.BaseApiService
                .GetAsync<StockDayResponseDto, StockDayRequestParamDto>(TwseApiUrl.StockDay, test);

            StockDayDto result = this.Mapper.Map<StockDayDto>(stockData);

            return (result, stockData.Stat);
        }
    }
}