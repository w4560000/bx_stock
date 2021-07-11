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

        /// <summary>
        /// 週六 Job
        /// 計算新個股 所有週KD
        /// 因是新股, 故重新計算週KD
        /// </summary>
        void CalcNewStockAllWeekKD();

        /// <summary>
        /// 計算個股 所有日KD
        /// Init資料時使用
        /// </summary>
        void CalcCurrentAllDayKD();

        /// <summary>
        /// 計算個股 所有週KD
        /// Init資料時使用
        /// </summary>
        void CalcCurrentAllWeekKD();

        /// <summary>
        /// 計算個股 該週KD
        /// 只能在週六 or 週日 執行
        /// </summary>
        void CalcCurrentWeekKD();
    }
}