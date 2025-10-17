using BX_Stock.Helper;
using BX_Stock.Models.Dto.StockDto;
using BX_Stock.Models.Entity;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock.Repository
{
    public class StockRepository
    {
        /// <summary>
        /// Log
        /// </summary>
        private readonly ILogger<StockRepository> _logger;

        /// <summary>
        /// DbConnection
        /// </summary>
        private readonly IDbConnection _dbConnection;

        public StockRepository(
            ILogger<StockRepository> logger,
            IDbConnection dbConnection)
        {
            this._logger = logger;
            this._dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Stock>> QueryStock(QueryStockDto query)
        {
            try
            {
                _logger.LogInformation($"QueryStock 執行, Param:{JsonConvert.SerializeObject(query)}");

                var dbResult = await _dbConnection.QueryAsync<Stock>("SP_Query_Stock", query, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("QueryStock 查詢結束");

                return dbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"QueryStock, 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

        public async Task<int> InsertUptStock(List<Stock> dataList)
        {
            try
            {
                _logger.LogInformation($"InsertStock 執行, 筆數:{dataList.Count}");

                var param = new
                {
                    UDT_Stock = dataList.ToDataTable().AsTableValuedParameter("UDT_Stock")
                };
                var dbResult = await _dbConnection.ExecuteAsync("SP_InsertUpt_Stock", param, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"InsertStock 執行結束");

                return dbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"InsertStock 查詢 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

        public async Task<int> InsertStockDay(List<StockDay> dataList)
        {
            try
            {
                _logger.LogInformation($"InsertStockDay 執行, 筆數:{dataList.Count}");

                var param = new
                {
                    UDT_StockDay = dataList.ToDataTable().AsTableValuedParameter("UDT_StockDay")
                };
                var dbResult = await _dbConnection.ExecuteAsync("SP_Insert_StockDay", param, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"InsertStockDay 執行結束");

                return dbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"InsertStockDay 查詢 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

        public async Task<int> InsertMovingAverage(List<MovingAverage> dataList)
        {
            try
            {
                _logger.LogInformation($"InsertMovingAverage 執行, 筆數:{dataList.Count}");

                var param = new
                {
                    UDT_MovingAverage = dataList.ToDataTable().AsTableValuedParameter("UDT_MovingAverage")
                };
                var dbResult = await _dbConnection.ExecuteAsync("SP_Insert_MovingAverage", param, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"InsertMovingAverage 執行結束");

                return dbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"InsertMovingAverage 查詢 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// 取得最新股票日資料 by day
        /// </summary>
        public async Task<List<StockDay>> GetStockDayDataByDay(int stockNo, int day)
        {
            try
            {
                _logger.LogInformation($"GetStockDayDataByDay 執行, 股票: {stockNo}, 天數: {day}");

                var sql = @"
                    SELECT TOP (@Day) * FROM StockDay 
                    WHERE StockNo = @StockNo
                    ORDER BY Date DESC";

                var result = await _dbConnection.QueryAsync<StockDay>(sql, new { StockNo = stockNo, Day = day });

                _logger.LogInformation($"GetStockDayDataByDay 執行結束, 取得 {result.Count()} 筆資料");

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetStockDayDataByDay 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// 取得股票日資料
        /// </summary>
        public async Task<List<StockDay>> GetStockDayData(int stockNo, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"GetStockDayData 執行, 股票: {stockNo}, 期間: {startDate:yyyy-MM-dd} ~ {endDate:yyyy-MM-dd}");

                var sql = @"
                    SELECT * FROM StockDay 
                    WHERE StockNo = @StockNo 
                    AND Date >= @StartDate 
                    AND Date <= @EndDate 
                    ORDER BY Date";

                var result = await _dbConnection.QueryAsync<StockDay>(sql, new { StockNo = stockNo, StartDate = startDate, EndDate = endDate });

                _logger.LogInformation($"GetStockDayData 執行結束, 取得 {result.Count()} 筆資料");

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetStockDayData 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }

    }
}