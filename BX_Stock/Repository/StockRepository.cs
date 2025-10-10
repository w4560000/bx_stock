using BX_Stock.Helper;
using BX_Stock.Models.Dto.StockDto;
using BX_Stock.Models.Entity;
using BX_Stock.Service;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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

                _logger.LogInformation($"InsertStock 執行結束, 新增筆數:{dbResult}");

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

                _logger.LogInformation($"InsertStockDay 執行結束, 新增筆數:{dbResult}");

                return dbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"InsertStockDay 查詢 發生錯誤, Error: {ex.Message}.");
                throw;
            }
        }
    }
}