using System.Collections.Generic;

namespace BX_Stock.Service
{
    /// <summary>
    /// 股票Service
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// 計算週KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        void ProcessStockWeekKD(string stockNo);

        /// <summary>
        /// 計算月KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        void ProcessStockMonthKD(string stockNo);

        /// <summary>
        /// 刪除個股資訊
        /// </summary>
        /// <param name="deleteStockNoList">欲刪除的個股代號清單</param>
        void DeleteStockData(List<string> deleteStockNoList);
    }
}