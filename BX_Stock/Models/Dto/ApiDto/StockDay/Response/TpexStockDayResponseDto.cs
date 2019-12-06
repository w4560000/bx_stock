using System.Collections.Generic;

namespace BX_Stock.Models.Dto
{
    public class TpexStockDayResponseDto
    {
        /// <summary>
        /// 個股代號
        /// </summary>
        public string StkNo { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string StkName { get; set; }

        /// <summary>
        /// 無使用
        /// </summary>
        public bool ShowListPriceNote { get; set; }

        /// <summary>
        /// 無使用
        /// </summary>
        public bool ShowListPriceLink { get; set; }

        /// <summary>
        /// 查詢時間
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int ITotalRecords { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public List<List<string>> AaData { get; set; }
    }
}