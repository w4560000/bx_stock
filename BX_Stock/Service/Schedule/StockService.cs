using BX_Stock.Helper;
using BX_Stock.Models.Entity;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BX_Stock.Service
{
    /// <summary>
    /// 股票Service
    /// </summary>
    public class StockService : IStockService
    {
        /// <summary>
        /// DbContext
        /// </summary>
        private readonly StockContext StockContext;

        /// <summary>
        /// 網路爬蟲 Service
        /// </summary>
        private readonly IWebCrawlerService WebCrawlerService;

        /// <summary>
        /// 證交所API Service
        /// </summary>
        private readonly ITwseAPIService TwseAPIService;

        /// <summary>
        /// 櫃買中心API Service
        /// </summary>
        private readonly ITpexAPIService TpexAPIService;

        /// <summary>
        /// 個股Service
        /// </summary>
        /// <param name="stockContext">DbContext</param>
        /// <param name="webCrawlerService">網路爬蟲Service</param>
        /// <param name="twseAPIService">證交所API Service</param>
        /// <param name="tpexAPIService">櫃買中心API Service</param>
        public StockService(
            StockContext stockContext,
            IWebCrawlerService webCrawlerService,
            ITwseAPIService twseAPIService,
            ITpexAPIService tpexAPIService)
        {
            this.StockContext = stockContext;
            this.WebCrawlerService = webCrawlerService;
            this.TwseAPIService = twseAPIService;
            this.TpexAPIService = tpexAPIService;
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

            List<int> allStockNo = allStockData.Select(s => s.StockNo).ToList();
            List<int> currentDbStockNo = currentDbStockData.Select(s => s.StockNo).ToList();

            // 目前DB沒有的個股，但市面上有的(新上市or新上櫃)，新增該股資料
            List<int> insertStockNoList = allStockNo.Except(currentDbStockNo).ToList();
            this.InsertStock(allStockData.Where(x => insertStockNoList.Contains(x.StockNo)).ToList());

            //// 目前DB有的個股，但市面上沒有的(下市or下櫃)，刪除該股在DB的資料
            List<int> deleteStockNoList = currentDbStockNo.Except(allStockNo).ToList();
            this.DeleteStockData(deleteStockNoList);
        }

        public void Test()
        {
            foreach (int i in this.StockContext.Set<Stock>().Select(s => s.StockNo).ToList())
            {
                this.ProcessStockDayKD(i);
                this.ProcessStockWeekKD(i);
                this.ProcessStockMonthKD(i);
            }
        }

        /// <summary>
        /// 新增個股資訊
        /// </summary>
        /// <param name="stockNoList">要新增的個股代號</param>
        private void InsertStock(List<Stock> insertStockList)
        {
            // 新增個股代號
            this.StockContext.BulkInsert(insertStockList);

            // 上市個股代號
            List<int> insertTwseStockNo = insertStockList.Where(w => w.IsListed).Select(s => s.StockNo).ToList();

            // 上櫃個股代號
            List<int> insertTpexStockNo = insertStockList.Where(w => !w.IsListed).Select(s => s.StockNo).ToList();

            // 撈上市個股歷史資料
            insertTwseStockNo.ForEach(x => this.TwseAPIService.ProcessStockHistoryData(x));

            // 撈上櫃個股歷史資料
            insertTpexStockNo.ForEach(x => this.TpexAPIService.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            //insertStockNoList.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            //insertStockNoList.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        private void ProcessStockDayKD(int stockNo)
        {
            // 取出個股每日資訊
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo)).ToList();

            List<IStockEntity> a = new List<IStockEntity>();
            stockData.ForEach(x => a.Add(x));
            a.CalcKD();
            this.StockContext.BulkUpdate(a);
        }
        /// <summary>
        /// 計算週KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        private void ProcessStockWeekKD(int stockNo)
        {
            // 取出個股每日資訊
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo)).ToList();
            List<List<StockDay>> stockDataListOfWeeks = new List<List<StockDay>>();

            // 以年做區隔 一年一年處理
            foreach (List<StockDay> stockDayList in stockData.GroupBy(x => x.Date.Year).Select(s => s.ToList()).ToList())
            {
                // 以週做區隔
                List<List<StockDay>> stockDataOfWeek = stockDayList.GroupBy(x => new GregorianCalendar().GetWeekOfYear(x.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                                                                   .Select(s => s.ToList())
                                                                   .ToList();

                stockDataListOfWeeks.AddRange(stockDataOfWeek);
            }

            // 轉換成Entity並計算週KD
            List<IStockEntity> stockWeek = stockDataListOfWeeks.AsStockWeekOrMonth<StockWeek>().CalcKD();
            this.StockContext.BulkInsert(stockWeek);
        }

        /// <summary>
        /// 計算月KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        private void ProcessStockMonthKD(int stockNo)
        {
            // 取出個股每日資訊
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo)).ToList();
            List<List<StockDay>> stockDataListOfMonths = new List<List<StockDay>>();

            // 以年做區隔 一年一年處理
            foreach (List<StockDay> stockDayList in stockData.GroupBy(x => x.Date.Year).Select(s => s.ToList()).ToList())
            {
                // 以月份做區隔
                List<List<StockDay>> stockDataListOfMonth = stockDayList.GroupBy(x => x.Date.Month)
                                                                        .Select(s => s.ToList())
                                                                        .ToList();

                stockDataListOfMonths.AddRange(stockDataListOfMonth);
            }

            // 轉換成Entity並計算月KD
            List<IStockEntity> stockMonth = stockDataListOfMonths.AsStockWeekOrMonth<StockMonth>().CalcKD();
            this.StockContext.BulkInsert(stockMonth);
        }

        /// <summary>
        /// 刪除個股資訊
        /// </summary>
        /// <param name="deleteStockNoList">欲刪除的個股代號清單</param>
        private void DeleteStockData(List<int> deleteStockNoList)
        {
            List<Stock> deleteStock = this.StockContext.Set<Stock>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockDay> deleteStockDay = this.StockContext.Set<StockDay>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockWeek> deleteStockWeek = this.StockContext.Set<StockWeek>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockMonth> deleteStockMonth = this.StockContext.Set<StockMonth>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();

            using var transaction = this.StockContext.Database.BeginTransaction();
            this.StockContext.BulkDelete(deleteStock);
            this.StockContext.BulkDelete(deleteStockDay);
            this.StockContext.BulkDelete(deleteStockWeek);
            this.StockContext.BulkDelete(deleteStockMonth);

            transaction.Commit();
        }
    }
}