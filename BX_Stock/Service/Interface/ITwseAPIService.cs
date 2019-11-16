using BX_Stock.Models.Dto.TwseDto;
using System;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        /// <summary>
        /// 每日排程 撈取現有股號
        /// 若撈取的資料與現有資料庫股號有差異 
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockSchedule1();

        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockScheduleFirst(int start, int end);
    }
}