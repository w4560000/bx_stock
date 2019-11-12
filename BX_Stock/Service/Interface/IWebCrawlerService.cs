using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    /// <summary>
    /// 爬蟲Interface
    /// </summary>
    public interface IWebCrawlerService
    {
        /// <summary>
        /// 爬取現有上市的股票代號
        /// </summary>
        /// <returns>現有上市的股票代號</returns>
        Task<List<Stock>> GetAllListedStockNoAsync();

        /// <summary>
        /// 爬取現有上櫃的股票代號
        /// </summary>
        /// <returns>現有上櫃的股票代號</returns>
        Task<List<Stock>> GetAllCabinetStockNoAsync();
    }
}