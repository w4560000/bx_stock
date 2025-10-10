using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using BX_Stock.Repository;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IBaseApiService _baseApiService;

        /// <summary>
        /// AutoMapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<TwseAPIService> _logger;

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
            this._baseApiService = baseApiService;
            this._mapper = mapper;
            this._logger = logger;
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

        /// <summary>
        /// 取得上市個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        public async Task<List<StockDay>> GetStockHistoryData(int stockNo, string startMonth = "2025-06")
        {
            Stopwatch sw = Stopwatch.StartNew();
            _logger.LogInformation($"GetStockHistoryData Start, 個股: {stockNo}, 查詢月份: {startMonth}, 耗時:{sw.ElapsedMilliseconds}ms");

            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            var stockDayList = new List<StockDay>();

            var monthList = twseDataStartMonth.EachMonthTo(DateTime.Now);
            foreach (DateTime date in monthList)
            {
                (StockDayDto stockDayDto, bool hasGetData) = await this.GetStockDataAsync(stockNo, date);
                this._logger.LogInformation($"GetStockHistoryData 個股: {stockNo}, 查詢月份: {date:yyyy-MM}, 耗時:{sw.ElapsedMilliseconds}ms");

                if (!hasGetData)
                    continue;

                stockDayList.AddRange(this._mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            var dateNow = DateTime.Now;

            stockDayList.ForEach(f =>
            {
                f.StockNo = stockNo;
                f.CreateDate = dateNow;
            });

            var 起始時間 = monthList.Min().ToString("yyyy-MM");
            var 結束時間 = monthList.Max().ToString("yyyy-MM");
            this._logger.LogInformation($"GetStockHistoryData 個股: {stockNo}, 總筆數: {stockDayList.Count}, 時間區間: {起始時間}~{結束時間}, 耗時:{sw.ElapsedMilliseconds}ms 結束");

            return stockDayList;
        }

        /// <summary>
        /// 取得上市個股日資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="date">愈新增的日期</param>
        public async Task<StockDay> GetStockDayData(int stockNo, DateTime date)
        {
            Stopwatch sw = Stopwatch.StartNew();
            this._logger.LogInformation($"GetStockDayData 個股日資料, 個股: {stockNo}, 日期: {date:yyyy-MM-dd}, 耗時:{sw.ElapsedMilliseconds}ms");

            var stockDayData = await this.GetStockHistoryData(stockNo, date.ToString("yyyy-MM"));

            var todayStockDay = stockDayData.Where(w => w.Date.ToString("yyyyMMdd") == date.ToString("yyyyMMdd")).FirstOrDefault();

            this._logger.LogInformation($"GetStockDayData 個股日資料, 個股: {stockNo}, 日期: {date:yyyy-MM-dd}, 耗時:{sw.ElapsedMilliseconds}ms 結束");

            return todayStockDay;
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        private async Task<(StockDayDto, bool)> GetStockDataAsync(int stockNo, DateTime date)
        {
            StockDayDto result = null;
            TwseStockDayResponseDto stockData = new TwseStockDayResponseDto();
            try
            {
                TwseStockDayRequestParamDto requestParam = new TwseStockDayRequestParamDto()
                {
                    Date = date.ToString("yyyyMMdd"),  //"20190901",
                    StockNo = stockNo.ToString()
                };

                while (true)
                {
                    // 連續串證交所API 會被鎖IP，五秒內限制3次請求
                    System.Threading.Thread.Sleep(2000);

                    stockData = await this._baseApiService
                        .GetAsync<TwseStockDayResponseDto, TwseStockDayRequestParamDto>(StockApiUrl.TwseStockDay, requestParam);

                    _logger.LogInformation($"GetStockDataAsync: 股號:{stockNo}, 查詢date:{date:yyyyMMdd}.");

                    if (stockData == null)
                    {
                        _logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date:yyyyMMdd}, stockData = null");
                        continue;
                    }

                    switch (stockData.Stat)
                    {
                        case "OK":
                            result = this._mapper.Map<StockDayDto>(stockData);
                            if (result.Data.FirstOrDefault().Date.ToString("yyyyMM") == date.ToString("yyyyMM"))
                                return (result, true);
                            break;

                        case "很抱歉，沒有符合條件的資料!":
                            _logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date:yyyyMMdd}, {stockData.Stat}");
                            return (null, false);

                        case "查詢日期小於99年1月4日，請重新查詢!":
                        case "查詢日期大於今日，請重新查詢!":

                            _logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date:yyyyMMdd}, {stockData.Stat} 已重試");
                            break;

                        default:
                            throw new Exception(stockData.Stat);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"新增上市個股 發生錯誤: 股號:{stockNo}, date:{date:yyyyMMdd}, error: {ex}.");
                throw ex;
            }
        }
    }
}