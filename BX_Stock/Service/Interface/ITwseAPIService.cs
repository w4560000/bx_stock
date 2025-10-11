using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        /// <summary>
        /// (Schedule2)
        /// </summary>
        /// <returns>股票代號清單</returns>
        void ProcessStockScheduleFirst(int start, int end);

        /// <summary>
        /// 取得上市個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        Task<List<StockDay>> GetStockHistoryData(int stockNo, string startMonth = "2025-06", string endMonth = "2025-09");


        /// <summary>
        /// 取得上市個股日資料
        /// </summary>
        /// <param name="stockNo"要新增的個股></param>
        /// <param name="date">查詢起始時間</param>
        Task<StockDay> GetStockDayData(int stockNo, DateTime date);


        /// <summary>
        /// 取得上市個股最新日資料
        /// </summary>
        Task<List<StockDay>> GetStockNewDayData(List<int> 目標個股清單);
    }
}