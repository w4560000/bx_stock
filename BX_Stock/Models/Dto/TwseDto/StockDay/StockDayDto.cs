using System;

namespace BX_Stock.Models.Dto
{
    public class StockDayDto<T>
    {
        /// <summary>
        /// 回傳狀態
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string StockNo { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public T[] Data { get; set; }
    }
}