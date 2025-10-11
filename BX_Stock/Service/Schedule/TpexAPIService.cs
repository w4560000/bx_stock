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
        public void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01-04", string endMonth = "2021-06")
        {
            // todo
            //this.Logger.LogInformation($"新增上櫃個股 : {stockNo} start!");
            //DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            //List<IStockEntity> result = new List<IStockEntity>();

            //foreach (DateTime date in twseDataStartMonth.EachMonthTo(DateTime.Parse(endMonth)))
            //{
            //    //(StockDayDto stockDayDto, int recordCount) = RetryHelper.RetryIfThrown<Exception, (StockDayDto, int)>(() =>
            //    //                                                    this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult(), 3, 3);

            //    (StockDayDto stockDayDto, int recordCount) = this.GetStockDataAsync(stockNo, date).GetAwaiter().GetResult();

            //    if (recordCount < 1)
            //    {
            //        continue;
            //    }

            //    result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
            //}

            //result.ForEach(f => f.StockNo = stockNo);

            //// 計算KD
            //result.CalcAllKD();

            //this.StockContext.AddRange(result);
            //this.StockContext.SaveChanges();
            //this.Logger.LogInformation($"新增上櫃個股 : {stockNo} end!");
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
        private async Task<(StockDayDto, int)> GetStockDataAsync(int stockNo, DateTime date)
        {
            try
            {
                TpexStockDayRequestParamDto test = new TpexStockDayRequestParamDto()
                {
                    D = date.ConvertToTaiwanType(),  // "108/09",
                    Stkno = stockNo.ToString()
                };

                while (true)
                {
                    // 連續串證交所API 會被鎖IP，五秒內限制3次請求
                    System.Threading.Thread.Sleep(2000);

                    TpexStockDayResponseDto stockData = await this._baseApiService
                        .GetAsync<TpexStockDayResponseDto, TpexStockDayRequestParamDto>(StockApiUrl.TpexStockDay, test);

                    if (stockData == null)
                    {
                        _logger.LogError($"新增上櫃個股失敗: 股號:{stockNo}, 查詢date:{date}, stockData = null");
                        continue;
                    }

                    StockDayDetailDto[] detailDto = new StockDayDetailDto[stockData.ITotalRecords];

                    // mapping
                    for (int i = 0; i < stockData.ITotalRecords; i++)
                    {
                        detailDto[i] = new StockDayDetailDto
                        {
                            Date = DateTime.Parse(stockData.AaData[i][0].ConvertToADType().Substring(0, 10)),
                            TradeVolume = Convert.ToInt32(stockData.AaData[i][1].Replace(",", string.Empty)),
                            TradeValue = Convert.ToInt64(stockData.AaData[i][2].Replace(",", string.Empty)),
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
            catch (Exception ex)
            {
                this._logger.LogError($"新增上櫃個股 發生錯誤: 股號:{stockNo}, date:{date}, error: {ex}.");
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