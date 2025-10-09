using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
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
        /// AutoMapper
        /// </summary>
        private readonly IMapper Mapper;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<TwseAPIService> Logger;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="baseApiService">基礎APIService</param>
        /// <param name="stockContext">DbContext</param>
        /// <param name="mapper">AutoMapper</param>
        public TwseAPIService(
            IBaseApiService baseApiService,
            IMapper mapper,
            ILogger<TwseAPIService> logger)
        {
            this.BaseApiService = baseApiService;
            this.Mapper = mapper;
            this.Logger = logger;
        }

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockScheduleFirst(int start, int end)
        {
            // todo
            //List<int> currentDbStockNo = this.StockContext.Set<Stock>().Where(w => w.IsListed && w.IsEnabled).Select(s => s.StockNo).ToList();
            //currentDbStockNo = currentDbStockNo.Where(w => start <= w && w < end).ToList();

            //// 撈歷史資料
            //currentDbStockNo.ForEach(x => this.ProcessStockHistoryData(x));

            // 計算週KD 月KD
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockWeekKD(x));
            //currentDbStockNo.ForEach(x => this.StockService.ProcessStockMonthKD(x));
        }

        ///// <summary>
        ///// 新增上市個股歷史資料
        ///// </summary>
        ///// <param name="stockNo">要新增的個股</param>
        ///// <param name="startMonth">查詢起始時間</param>
        //public void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01-04")
        //{
        //    this.Logger.LogInformation($"ProcessStockHistoryData 新增上市個股 : {stockNo} start!");
        //    DateTime twseDataStartMonth = DateTime.Parse(startMonth);
        //    List<IStockEntity> result = new List<IStockEntity>();

        //    foreach (DateTime date in twseDataStartMonth.EachMonthTo(DateTime.Now))
        //    {
        //        (StockDayDto stockDayDto, bool hasGetData) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

        //        if (!hasGetData)
        //        {
        //            continue;
        //        }

        //        result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
        //    }

        //    result.ForEach(f => f.StockNo = stockNo);

        //    // 計算KD
        //    result.CalcAllKD();

        //    result.Remove(result.Where(w => w.Date.ToString("yyyyMMdd") == "20210716").FirstOrDefault());

        //    this.StockContext.BulkInsertOrUpdate(result);
        //    this.Logger.LogInformation($"ProcessStockHistoryData 新增上市個股 : {stockNo} end!");
        //}

        ///// <summary>
        ///// 新增上市個股當日資料
        ///// </summary>
        ///// <param name="stockNo">要新增的個股</param>
        //public void ProcessStockToDayData(int stockNo)
        //{
        //    this.Logger.LogInformation($"ProcessStockToDayData 上市個股當日資料 : {stockNo} start!");
        //    List<IStockEntity> result = new List<IStockEntity>();

        //    var date = DateTime.Parse("2021-07-16");

        //    (StockDayDto stockDayDto, bool hasGetData) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

        //    if (!hasGetData)
        //    {
        //        this.Logger.LogInformation($"ProcessStockToDayData 上市個股當日資料失敗 個股:{stockNo} hasGetData = false, 查無資料");
        //    }

        //    var todayStockDay = this.Mapper.Map<List<StockDay>>(stockDayDto.Data).Where(w => w.Date.ToString("yyyyMMdd") == date.ToString("yyyyMMdd")).FirstOrDefault();
        //    todayStockDay.StockNo = stockNo;

        //    if (todayStockDay != null)
        //    {
        //        // 取得該個股前八日 StockDay資料
        //        List<StockDay> stockDataList = this.StockContext
        //                                           .Set<StockDay>()
        //                                           .Where(w => w.StockNo.Equals(stockNo))
        //                                           .OrderByDescending(o => o.Date)
        //                                           .Take(8)
        //                                           .ToList();

        //        stockDataList.Add(todayStockDay);

        //        List<IStockEntity> stockDayList = new List<IStockEntity>();
        //        stockDayList.AddRange(stockDataList);
        //        stockDayList = stockDayList.OrderBy(o => o.Date).ToList();
        //        stockDayList.CalcCurrentKD(9);

        //        this.StockContext.BulkInsertOrUpdate(new List<IStockEntity>() { stockDayList.Last() });
        //        this.Logger.LogInformation($"ProcessStockToDayData 上市個股當日資料 End!");
        //    }
        //    else
        //    {
        //        this.Logger.LogInformation($"ProcessStockToDayData 上市個股當日資料失敗 個股:{stockNo} 查無今日資料");
        //    }
        //}

        ///// <summary>
        ///// 取得個股單月資訊
        ///// </summary>
        ///// <returns>個股單月資訊</returns>
        //private async Task<(StockDayDto, bool)> GetStockDataAsync(int stockNo, DateTime date)
        //{
        //    StockDayDto result = null;
        //    TwseStockDayResponseDto stockData = new TwseStockDayResponseDto();
        //    try
        //    {
        //        TwseStockDayRequestParamDto requestParam = new TwseStockDayRequestParamDto()
        //        {
        //            Date = date.ToString("yyyyMMdd"),  //"20190901",
        //            StockNo = stockNo.ToString()
        //        };

        //        while (true)
        //        {
        //            // 連續串證交所API 會被鎖IP，每隔三秒串一次
        //            System.Threading.Thread.Sleep(3000);

        //            stockData = await this.BaseApiService
        //                .GetAsync<TwseStockDayResponseDto, TwseStockDayRequestParamDto>(StockApiUrl.TwseStockDay, requestParam);

        //            Logger.LogInformation($"GetStockDataAsync: 股號:{stockNo}, 查詢date:{date}.");

        //            if (stockData == null)
        //            {
        //                Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, stockData = null");
        //                continue;
        //            }

        //            switch (stockData.Stat)
        //            {
        //                case "OK":
        //                    result = this.Mapper.Map<StockDayDto>(stockData);
        //                    if (result.Data.FirstOrDefault().Date.ToString("yyyyMM") == date.ToString("yyyyMM"))
        //                        return (result, true);
        //                    break;

        //                case "很抱歉，沒有符合條件的資料!":
        //                    Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, {stockData.Stat}");
        //                    return (null, false);

        //                case "查詢日期小於99年1月4日，請重新查詢!":
        //                case "查詢日期大於今日，請重新查詢!":

        //                    Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, {stockData.Stat} 已重試");
        //                    break;

        //                default:
        //                    throw new Exception(stockData.Stat);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Logger.LogError($"新增上市個股 發生錯誤: 股號:{stockNo}, date:{date}, error: {ex}.");
        //        throw ex;
        //    }
        //}
    }
}