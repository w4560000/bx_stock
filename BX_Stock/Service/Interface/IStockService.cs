using Hangfire.MemoryStorage.Database;
using System;
using System.Threading.Tasks;

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
        Task ProcessNewStock_Schedule1(DateTime date);

        ///// <summary>
        ///// 每日排程 新增當日個股 (Schedule2)
        ///// </summary>
        //Task ProcessTodayStock_Schedule2(DateTime date);

        /// <summary>
        /// 處理個股歷史資料 (初始化)
        /// </summary>
        Task ProcessStockHistoryData();

        /// <summary>
        /// 重撈個股日資料 (補資料用)
        /// </summary>
        Task ProcessStockDay(DateTime date);

        ///// <summary>
        ///// 週六 Job
        ///// 計算新個股 所有週KD
        ///// 因是新股, 故重新計算週KD
        ///// </summary>
        //void CalcNewStockAllWeekKD(DateTime date);

        ///// <summary>
        ///// 計算個股 所有日KD
        ///// Init資料時使用
        ///// </summary>
        //void CalcCurrentAllDayKD();

        ///// <summary>
        ///// 計算個股 所有週KD
        ///// Init資料時使用
        ///// </summary>
        //void CalcCurrentAllWeekKD();

        ///// <summary>
        ///// 計算個股 該週KD
        ///// 只能在週六 or 週日 執行
        ///// </summary>
        //void CalcCurrentWeekKD();
    }
}