using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IBaseApiService _baseApiService;

        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<TpexAPIService> _logger;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="baseApiService">基礎APIService</param>
        /// <param name="stockContext">DbContext</param>
        /// <param name="mapper">Mapper</param>
        public TpexAPIService(
            IBaseApiService baseApiService,
            IMapper mapper,
            ILogger<TpexAPIService> logger)
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
            //List<int> currentDbStockNo = this.StockContext.Set<Stock>().Where(w => !w.IsListed).Select(s => s.StockNo).ToList();
            //currentDbStockNo = currentDbStockNo.Where(w => start <= w && w < end).ToList();

            //// 撈歷史資料
            //currentDbStockNo.ForEach(x => this.ProcessStockHistoryData(x));

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
        public async Task<List<StockDay>> GetStockHistoryData(int stockNo, string startMonth = "2025-06", string endMonth = "2025-09")
        {
            Stopwatch sw = Stopwatch.StartNew();
            _logger.LogInformation($"GetStockHistoryData Start, 個股: {stockNo}, 查詢月份: {startMonth}~{endMonth}, 耗時:{sw.ElapsedMilliseconds}ms");

            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            DateTime twseDataEndMonth = DateTime.Parse(endMonth);
            var monthList = twseDataStartMonth.EachMonthTo(twseDataEndMonth);

            var stockDayList = new List<StockDay>();

            foreach (DateTime date in monthList)
            {
                (StockDayDto<TpexStockDayDetailDto> stockDayDto, bool hasGetData) = await this.GetStockDataAsync(stockNo, date);

                if (!hasGetData)
                    continue;

                stockDayList.AddRange(this._mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            var dateNow = DateTime.Now;

            stockDayList.ForEach(f =>
            {
                f.StockNo = stockNo;
                f.CreateDate = dateNow;

                f.TradeVolume = f.TradeVolume * 1000; // 單位是張，*1000 轉成股 存入DB
                f.TradeValue = f.TradeValue * 1000; // 單位是千元，*1000 存入DB
            });

            var 起始時間 = monthList.Min().ToString("yyyy-MM");
            var 結束時間 = monthList.Max().ToString("yyyy-MM");
            this._logger.LogInformation($"GetStockHistoryData 個股: {stockNo}, 總筆數: {stockDayList.Count}, 時間區間: {起始時間}~{結束時間}, 耗時:{sw.ElapsedMilliseconds}ms 結束");

            return stockDayList;
        }

        /// <summary>
        /// 取得上櫃個股日資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="date">欲新增的日期</param>
        public async Task<StockDay> GetStockDayData(int stockNo, DateTime date)
        {
            Stopwatch sw = Stopwatch.StartNew();
            this._logger.LogInformation($"GetStockDayData 個股日資料, 個股: {stockNo}, 日期: {date:yyyy-MM-dd}, 耗時:{sw.ElapsedMilliseconds}ms");

            var stockDayData = await this.GetStockHistoryData(stockNo, date.ToString("yyyy-MM"), date.ToString("yyyy-MM"));

            var todayStockDay = stockDayData.Where(w => w.Date.ToString("yyyyMMdd") == date.ToString("yyyyMMdd")).FirstOrDefault();

            this._logger.LogInformation($"GetStockDayData 個股日資料, 個股: {stockNo}, 日期: {date:yyyy-MM-dd}, 耗時:{sw.ElapsedMilliseconds}ms 結束");

            return todayStockDay;
        }

        /// <summary>
        /// 取得上櫃個股最新日資料
        /// </summary>
        public async Task<List<StockDay>> GetStockNewDayData(List<int> currentDbStockNo)
        {
            Stopwatch sw = Stopwatch.StartNew();
            this._logger.LogInformation($"GetStockNewDayData 上櫃個股最新日資料, 耗時:{sw.ElapsedMilliseconds}ms");

            (List<StockDay> stockDayList, bool hasGetData) = await this.GetStockNewDayDataAsync(currentDbStockNo);

            if (!hasGetData)
                return new List<StockDay>();

            this._logger.LogInformation($"GetStockNewDayData 上櫃個股最新日資料, 耗時:{sw.ElapsedMilliseconds}ms 結束");

            var dateNow = DateTime.Now;

            stockDayList.ForEach(f =>
            {
                f.CreateDate = dateNow;
            });

            return stockDayList;
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        private async Task<(StockDayDto<TpexStockDayDetailDto>, bool)> GetStockDataAsync(int stockNo, DateTime date)
        {
            StockDayDto<TpexStockDayDetailDto> result = null;

            try
            {
                TpexStockDayRequestParamDto param = new TpexStockDayRequestParamDto()
                {
                    Date = date.ToString("yyyy/MM/dd"),
                    Code = stockNo.ToString()
                };

                while (true)
                {
                    // 連續串證交所API 會被鎖IP，五秒內限制3次請求
                    System.Threading.Thread.Sleep(2000);

                    TpexStockDayResponseDto stockData = await this._baseApiService
                        .GetAsync<TpexStockDayResponseDto, TpexStockDayRequestParamDto>(StockApiUrl.TpexStockDay, param);

                    if (stockData == null)
                    {
                        _logger.LogError($"GetStockDataAsync 失敗, 個股:{stockNo}, 查詢date:{date:yyyyMMdd}, stockData = null");
                        continue;
                    }

                    switch (stockData.Stat.ToLower())
                    {
                        case "ok":
                            result = this._mapper.Map<StockDayDto<TpexStockDayDetailDto>>(stockData);
                            if (result.Data.FirstOrDefault().Date.ToString("yyyyMM") == date.ToString("yyyyMM"))
                                return (result, true);
                            break;

                        case "很抱歉，沒有符合條件的資料!":
                            _logger.LogError($"GetStockDataAsync 失敗 {stockData.Stat}, 個股:{stockNo}, 查詢date:{date:yyyyMMdd}");
                            return (null, false);

                        default:
                            throw new Exception(stockData.Stat);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"GetStockDataAsync 發生錯誤: 股號:{stockNo}, date:{date}, error: {ex}.");
                throw ex;
            }
        }

        /// <summary>
        /// 取得上櫃個股最新日資料
        /// </summary>
        private async Task<(List<StockDay>, bool)> GetStockNewDayDataAsync(List<int> currentDbStockNo)
        {
            List<StockDay> result = null;
            var stockData = new List<TpexStockDayAllResponseDto>();
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                _logger.LogInformation($"GetStockNewDayDataAsync Start, 耗時:{sw.ElapsedMilliseconds}ms");

                var dbStockNo = currentDbStockNo.Select(s => s.ToString()).ToList();
                stockData = await this._baseApiService
                        .GetAsync<List<TpexStockDayAllResponseDto>>(StockApiUrl.TpexStockNewDay);

                _logger.LogInformation($"GetStockNewDayDataAsync 撈完API, 耗時:{sw.ElapsedMilliseconds}ms");

                if (stockData == null || !stockData.Any())
                {
                    _logger.LogError($"GetStockNewDayDataAsync 失敗, stockData 查無資料");
                    return (null, false);
                }

                stockData = stockData.Where(w => dbStockNo.Contains(w.SecuritiesCompanyCode)).ToList();
                stockData = stockData.Where(w => !string.IsNullOrWhiteSpace(w.SecuritiesCompanyCode)).ToList();

                #region 檢查欄位 移除異常資料

                var removeStockNoList = new List<string>();
                foreach (var item in stockData)
                {
                    if (string.IsNullOrWhiteSpace(item.Date))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} Date異常");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.TradingShares) || !long.TryParse(item.TradingShares, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} TradingShares 異常 {item.TradingShares}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.TransactionAmount) || !long.TryParse(item.TransactionAmount, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} TransactionAmount 異常 {item.TransactionAmount}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Open) || !decimal.TryParse(item.Open, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} Open 異常 {item.Open}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.High) || !decimal.TryParse(item.High, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} High 異常 {item.High}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Low) || !decimal.TryParse(item.Low, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} Low 異常 {item.Low}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Close) || !decimal.TryParse(item.Close, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} Close 異常 {item.Close}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Change) || !decimal.TryParse(item.Change, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} Change 異常 {item.Change}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.TransactionNumber) || !long.TryParse(item.TransactionNumber, out _))
                    {
                        this._logger.LogError($"GetStockNewDayDataAsync 個股資料異常, 個股: {item.SecuritiesCompanyCode} TransactionNumber 異常 {item.TransactionNumber}");
                        removeStockNoList.Add(item.SecuritiesCompanyCode);
                        continue;
                    }
                }

                stockData = stockData.Where(w => !removeStockNoList.Contains(w.SecuritiesCompanyCode)).ToList();

                #endregion 檢查欄位 移除異常資料

                result = this._mapper.Map<List<StockDay>>(stockData);

                _logger.LogInformation($"GetStockNewDayDataAsync End, 耗時:{sw.ElapsedMilliseconds}ms");

                return (result, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"GetStockNewDayDataAsync 發生錯誤, error: {ex}.");
                throw ex;
            }
        }
    }
}