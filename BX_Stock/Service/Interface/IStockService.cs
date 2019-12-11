using System.Collections.Generic;

namespace BX_Stock.Service
{
    /// <summary>
    /// 股票Service
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// 每日排程 撈取現有股號 (Schedule1)
        /// 若撈取的資料與現有資料庫股號有差異
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockSchedule1();

        void Test();
    }
}