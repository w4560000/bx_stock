using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
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
        /// DbContext
        /// </summary>
        private readonly StockContext StockContext;

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
            StockContext stockContext,
            IMapper mapper,
            ILogger<TwseAPIService> logger)
        {
            this.BaseApiService = baseApiService;
            this.StockContext = stockContext;
            this.Mapper = mapper;
            this.Logger = logger;
        }

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void ProcessStockScheduleFirst(int start, int end)
        {
            List<int> currentDbStockNo = this.StockContext.Set<Stock>().Where(w => w.IsListed && w.IsEnabled).Select(s => s.StockNo).ToList();
            currentDbStockNo = currentDbStockNo.Where(w => start <= w && w < end).ToList();

            // 撈歷史資料
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
        public void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01-04", string endMonth = "2021-06")
        {
            this.Logger.LogInformation($"新增上市個股 : {stockNo} start!");
            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            List<IStockEntity> result = new List<IStockEntity>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(DateTime.Parse(endMonth)))
            {
                (StockDayDto stockDayDto, bool hasGetData) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

                if (!hasGetData)
                {
                    continue;
                }

                result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            result.ForEach(f => f.StockNo = stockNo);

            // 計算KD
            result.CalcAllKD();

            this.StockContext.AddRange(result);
            this.StockContext.SaveChanges();

            this.Logger.LogInformation($"新增上市個股 : {stockNo} end!");
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
                    // 連續串證交所API 會被鎖IP，每隔三秒串一次
                    System.Threading.Thread.Sleep(3000);

                    stockData = await this.BaseApiService
                        .GetAsync<TwseStockDayResponseDto, TwseStockDayRequestParamDto>(StockApiUrl.TwseStockDay, requestParam);

                    if (stockData == null)
                    {
                        Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, stockData = null");
                        continue;
                    }

                    switch (stockData.Stat)
                    {
                        case "OK":
                            result = this.Mapper.Map<StockDayDto>(stockData);
                            if (result.Data.FirstOrDefault().Date.ToString("yyyyMM") == date.ToString("yyyyMM"))
                                return (result, true);
                            break;

                        case "很抱歉，沒有符合條件的資料!":
                            Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, {stockData.Stat}");
                            return (null, false);

                        case "查詢日期小於99年1月4日，請重新查詢!":
                        case "查詢日期大於今日，請重新查詢!":

                            Logger.LogError($"新增上市個股失敗: 股號:{stockNo}, 查詢date:{date}, {stockData.Stat} 已重試");
                            break;

                        default:
                            throw new Exception(stockData.Stat);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"新增上市個股 發生錯誤: 股號:{stockNo}, date:{date}, error: {ex}.");
                throw ex;
            }
        }
    }
}