using BX_Stock.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    /// <summary>
    /// 上櫃個股 interface
    /// </summary>
    public interface ITpexAPIService
    {
        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockScheduleFirst(int start, int end);

        /// <summary>
        /// 新增上櫃個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        /// <param name="endMonth">查詢結束時間</param>
        void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01", string endMonth = "2019-10");


        /// <summary>
        /// 取得上櫃個股最新日資料
        /// </summary>
        Task<List<StockDay>> GetStockNewDayData(List<int> currentDbStockNo);
    }
}