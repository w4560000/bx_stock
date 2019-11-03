using BX_Stock.Models.Dto;
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
        /// 爬取全部股票代號
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>全部股票代號</returns>
        Task<List<StockNoDto>> GetAllStockNoAsync(string url);
    }
}