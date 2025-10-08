using BX_Stock.Helper;
using BX_Stock.Models.Entity;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        /// Log
        /// </summary>
        private readonly ILogger<StockService> Logger;

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
            ITpexAPIService tpexAPIService,
            ILogger<StockService> logger)
        {
            this.StockContext = stockContext;
            this.WebCrawlerService = webCrawlerService;
            this.TwseAPIService = twseAPIService;
            this.TpexAPIService = tpexAPIService;
            this.Logger = logger;
        }

        /// <summary>
        /// 每日排程 更新個股(新增新上市櫃個股&移除下市櫃個股) (Schedule1)
        /// 若撈取的資料與現有資料庫股號有差異
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        public async Task ProcessNewStock_Schedule1(DateTime date)
        {
            if (DateTimeHelper.IsHoliday(date))
            {
                this.Logger.LogInformation("ProcessNewStock_Schedule1 每日排程 更新個股 必須在平日執行");
                return;
            }

            try
            {
                Logger.LogInformation("ProcessNewStock_Schedule1 每日排程 更新個股 Start!");

                List<Stock> allStockData = new List<Stock>();

                // 爬取上市股票
                var getAllListedStockNoTask = this.WebCrawlerService.GetAllListedStockNoAsync();

                // 爬取上櫃股票
                var getAllCabinetStockNoTask = this.WebCrawlerService.GetAllCabinetStockNoAsync();

                allStockData.AddRange(await getAllListedStockNoTask);
                allStockData.AddRange(await getAllCabinetStockNoTask);

                List<int> allStockNo = allStockData.Select(s => s.StockNo).ToList();
                List<int> currentDbStockNo = this.StockContext.Set<Stock>().Select(s => s.StockNo).ToList();

                // 目前DB沒有的個股，但市面上有的(新上市or新上櫃)，新增該股資料
                List<int> insertStockNoList = allStockNo.Except(currentDbStockNo).ToList();
                this.InsertStock(allStockData.Where(x => insertStockNoList.Contains(x.StockNo)).ToList());

                //// 目前DB有的個股，但市面上沒有的(下市or下櫃)，刪除該股在DB的資料
                List<int> deleteStockNoList = currentDbStockNo.Except(allStockNo).ToList();
                this.DeleteStockData(deleteStockNoList);

                Logger.LogInformation("ProcessNewStock_Schedule1 每日排程 更新個股  End!");
            }
            catch (Exception ex)
            {
                Logger.LogError($"ProcessNewStock_Schedule1 每日排程 更新個股 發生錯誤, Error: {ex.Message}.");
            }
        }

        /// <summary>
        /// 每日排程 新增當日個股 (Schedule2)
        /// </summary>
        public void ProcessTodayStock_Schedule2(DateTime date)
        {
            if (DateTimeHelper.IsHoliday(date))
            {
                this.Logger.LogInformation("ProcessTodayStock_Schedule2 每日排程 新增當日個股 必須在平日執行");
                return;
            }

            using var transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this.StockContext.Database, System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                Logger.LogInformation("ProcessTodayStock_Schedule2 每日排程 新增當日個股 Start!");

                this.TwseAPIService.ProcessStockToDayData(1101);

                Logger.LogInformation("ProcessTodayStock_Schedule2 每日排程 新增當日個股 End!");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"ProcessTodayStock_Schedule2 每日排程 新增當日個股 發生錯誤, Error: {ex.Message}.");
                transaction.Rollback();
            }
        }

        /// <summary>
        /// 週六 Job
        /// 計算新個股 所有週KD
        /// 因是新股, 故重新計算週KD
        /// </summary>
        public void CalcNewStockAllWeekKD(DateTime date)
        {
            if (!DateTimeHelper.IsHoliday(date))
            {
                this.Logger.LogInformation("CalcNewStockAllWeekKD 計算新個股 所有週KD 必須在周六或周日執行");
                return;
            }

            using var transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this.StockContext.Database, System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (int i in this.StockContext.Set<Stock>().Where(w => w.IsEnabled).Select(s => s.StockNo).ToList())
                {
                    Logger.LogInformation($"CalcNewStockAllWeekKD 計算新個股 所有週KD 個股:{i} Start!");

                    this.ProcessStockAllWeekKD(i);

                    Logger.LogInformation($"CalcNewStockAllWeekKD 計算新個股 所有週KD 個股:{i} End!");
                }
                Logger.LogInformation("CalcNewStockAllWeekKD 計算新個股 所有週KD End!");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"CalcNewStockAllWeekKD 計算新個股 所有週KD 發生錯誤, Error: {ex.Message}.");
                transaction.Rollback();
            }
        }

        /// <summary>
        /// 計算個股 所有日KD
        /// Init資料時使用
        /// </summary>
        public void CalcCurrentAllDayKD()
        {
            using var transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this.StockContext.Database, System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (int i in this.StockContext.Set<Stock>().Where(w => w.IsEnabled).Select(s => s.StockNo).ToList())
                {
                    Logger.LogInformation($"CalcCurrentAllDayKD 計算個股 所有日KD 個股:{i} Start!");

                    // 取出個股每日資訊
                    List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(i)).ToList();

                    List<IStockEntity> stockDayList = new List<IStockEntity>();
                    stockData.ForEach(x => stockDayList.Add(x));
                    stockDayList.CalcAllKD();
                    this.StockContext.BulkInsertOrUpdate(stockDayList);

                    Logger.LogInformation($"CalcCurrentAllDayKD 計算個股 所有日KD 個股:{i} End!");
                }
                Logger.LogInformation("CalcCurrentAllDayKD 計算個股 所有日KD End!");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"CalcCurrentAllDayKD 計算個股 所有日KD 發生錯誤, Error: {ex.Message}.");
                transaction.Rollback();
            }
        }

        /// <summary>
        /// 計算個股 所有週KD
        /// Init資料時使用
        /// </summary>
        public void CalcCurrentAllWeekKD()
        {
            if (!(DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday))
            {
                this.Logger.LogInformation("CalcCurrentAllWeekKD 計算個股 所有週KD 必須在周六或周日執行");
                return;
            }

            using var transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this.StockContext.Database, System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (int i in this.StockContext.Set<Stock>().Where(w => w.IsEnabled).Select(s => s.StockNo).ToList())
                {
                    Logger.LogInformation($"CalcCurrentAllWeekKD 計算個股 所有週KD 個股:{i} Start!");

                    this.ProcessStockAllWeekKD(i);

                    Logger.LogInformation($"CalcCurrentAllWeekKD 計算個股 所有週KD 個股:{i} End!");
                }
                Logger.LogInformation("CalcCurrentAllWeekKD 計算個股 所有週KD End!");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"CalcCurrentAllWeekKD 計算個股 所有週KD 發生錯誤, Error:{ex.Message}");
                transaction.Rollback();
            }
        }

        /// <summary>
        /// 計算個股 該週KD
        /// 只能在週六 or 週日 執行
        /// </summary>
        public void CalcCurrentWeekKD()
        {
            using var transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this.StockContext.Database, System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (int i in this.StockContext.Set<Stock>().Where(w => w.IsEnabled).Select(s => s.StockNo).ToList())
                {
                    Logger.LogInformation($"CalcCurrentWeekKD 計算個股 該週KD 個股:{i} Start!");

                    this.ProcessStockCurrentWeekKD(i, 5);

                    Logger.LogInformation($"CalcCurrentWeekKD 計算個股 該週KD 個股:{i} End!");
                }
                Logger.LogInformation("CalcCurrentWeekKD 計算個股 該週KD End!");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"CalcCurrentWeekKD 計算個股 該週KD 發生錯誤, Error:{ex.Message}");
                transaction.Rollback();
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

            if (insertTwseStockNo.Count > 0)
                this.Logger.LogInformation($"InsertStock 新增個股資料 新上市個股: {insertTwseStockNo.Select(s => s.ToString()).Aggregate((current, total) => current + "," + total)} .");

            // 上櫃個股代號
            List<int> insertTpexStockNo = insertStockList.Where(w => !w.IsListed).Select(s => s.StockNo).ToList();

            if (insertTpexStockNo.Count > 0)
                this.Logger.LogInformation($"InsertStock 新增個股資料 新上櫃個股: {insertTpexStockNo.Select(s => s.ToString()).Aggregate((current, total) => current + "," + total)} .");

            // 撈上市個股歷史資料
            insertTwseStockNo.ForEach(x => this.TwseAPIService.ProcessStockHistoryData(x));

            // 撈上櫃個股歷史資料
            insertTpexStockNo.ForEach(x => this.TpexAPIService.ProcessStockHistoryData(x, endMonth: DateTime.Now.ToString("yyyy-MM")));
        }

        /// <summary>
        /// 計算個股 當前週KD
        /// 當週KD資料有誤時, 可清除DB週KD資料, 用該Method重新寫入
        /// 個股日資料 必須完整, 且需在週六or週日執行, 週KD 資料才正確
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        private void ProcessStockCurrentWeekKD(int stockNo, int n = 9)
        {
            // 取得當週 週一和週五日期
            (DateTime currentMondayOfWeekDate, DateTime currentFirDayOfWeekDate) = DateTime.Parse("2021-06-27").GetCurrentMondayAndFridayOfWeek();

            // 取得該個股當週 StockDay資料
            List<StockDay> stockData = this.StockContext
                                           .Set<StockDay>()
                                           .Where(w => w.StockNo.Equals(stockNo)
                                                    && w.Date >= currentMondayOfWeekDate
                                                    && w.Date <= currentFirDayOfWeekDate).ToList();

            // 取出該個股前n - 1週 StockWeek資料
            List<StockWeek> stockWeeks = this.StockContext
                                             .Set<StockWeek>()
                                             .Where(w => w.StockNo.Equals(stockNo))
                                             .OrderByDescending(o => o.Date)
                                             .Take(n - 1)
                                             .ToList();

            stockWeeks.Add(stockData.GetStockWeekOrMonthData<StockWeek>());

            List<IStockEntity> currentStockWeek = new List<IStockEntity>();
            currentStockWeek.AddRange(stockWeeks);
            currentStockWeek = currentStockWeek.OrderBy(o => o.Date).ToList();
            currentStockWeek.CalcCurrentKD(n);

            this.StockContext.Add(currentStockWeek.Last());
        }

        /// <summary>
        /// 計算個股 完整週KD
        /// 當週KD資料有誤時, 可清除DB週KD資料, 用該Method重新寫入
        /// 個股日資料 必須完整, 且需在週六or週日執行, 週KD 資料才正確
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        private void ProcessStockAllWeekKD(int stockNo)
        {
            // 取出個股每日資訊
            DateTime date = DateTime.Parse("2021-06-27");
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo) && w.Date <= date).ToList();
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
            List<IStockEntity> stockWeek = stockDataListOfWeeks.AsStockWeekOrMonth<StockWeek>().CalcAllKD(5);
            this.StockContext.BulkInsertOrUpdate(stockWeek);
        }

        /// <summary>
        /// 計算個股 完整月KD
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
            List<IStockEntity> stockMonth = stockDataListOfMonths.AsStockWeekOrMonth<StockMonth>().CalcAllKD(5);
            this.StockContext.BulkInsertOrUpdate(stockMonth);
        }

        /// <summary>
        /// 刪除個股資訊
        /// </summary>
        /// <param name="deleteStockNoList">欲刪除的個股代號清單</param>
        private void DeleteStockData(List<int> deleteStockNoList)
        {
            // 改為軟刪除 不刪除資料
            List<Stock> deleteStock = this.StockContext.Set<Stock>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            deleteStock.ForEach(x => x.IsEnabled = false);

            //List<StockDay> deleteStockDay = this.StockContext.Set<StockDay>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            //List<StockWeek> deleteStockWeek = this.StockContext.Set<StockWeek>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            //List<StockMonth> deleteStockMonth = this.StockContext.Set<StockMonth>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();

            this.StockContext.BulkUpdate(deleteStock);

            //this.StockContext.BulkDelete(deleteStock);
            //this.StockContext.BulkDelete(deleteStockDay);
            //this.StockContext.BulkDelete(deleteStockWeek);
            //this.StockContext.BulkDelete(deleteStockMonth);
        }
    }
}