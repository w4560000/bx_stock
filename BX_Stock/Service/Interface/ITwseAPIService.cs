namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockScheduleFirst(int start, int end);

        ///// <summary>
        ///// 新增上市個股歷史資料
        ///// </summary>
        ///// <param name="stockNo">要新增的個股</param>
        ///// <param name="startMonth">查詢起始時間</param>
        //void ProcessStockHistoryData(int stockNo, string startMonth = "2010-01-04");

        ///// <summary>
        ///// 新增上市個股當日資料
        ///// </summary>
        ///// <param name="stockNo">要新增的個股</param>
        //void ProcessStockToDayData(int stockNo);
    }
}