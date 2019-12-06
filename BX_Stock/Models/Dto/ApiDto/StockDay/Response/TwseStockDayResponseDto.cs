using Newtonsoft.Json;
using System.Collections.Generic;

namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 個股回傳查詢資訊Dto
    /// </summary>
    public class TwseStockDayResponseDto
    {
        /// <summary>
        /// 回傳狀態
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 個股資訊欄位
        /// </summary>
        public List<string> Fields { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public List<List<string>> Data { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        public List<string> Note { get; set; }
    }
}