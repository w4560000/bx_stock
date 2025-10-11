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
        /// 每日排程 撈取現有股號
        /// 若撈取的資料與現有資料庫股號有差異
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        /// <returns>股票代號清單</returns>
        Task 每日排程更新個股股號(DateTime date);

        /// <summary>
        /// 每日排程 撈最新上市個股資訊
        /// </summary>
        Task 每日排程撈最新上市個股資訊();

        /// <summary>
        /// 每日排程 撈最新上櫃個股資訊
        /// </summary>
        Task 每日排程撈最新上櫃個股資訊();

        /// <summary>
        /// 重撈上市個股歷史資訊 (初始化)
        /// </summary>
        Task 重撈上市個股歷史資訊();

        /// <summary>
        /// 重撈上櫃個股歷史資訊 (初始化)
        /// </summary>
        Task 重撈上櫃個股歷史資訊();

        /// <summary>
        /// 重撈上市個股日資訊 (補資料用)
        /// </summary>
        Task 重撈上市個股日資訊(DateTime date);

        /// <summary>
        /// 重撈上櫃個股日資訊 (補資料用)
        /// </summary>
        Task 重撈上櫃個股日資訊(DateTime date);

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