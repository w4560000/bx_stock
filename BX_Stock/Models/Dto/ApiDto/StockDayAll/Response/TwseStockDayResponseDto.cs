using Newtonsoft.Json;
using System.Collections.Generic;

namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 個股回傳查詢資訊Dto
    /// </summary>
    public class TwseStockDayAllResponseDto
    {
        /// <summary>
        /// 日期 (ex 1141009)
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 成交股數
        /// </summary>
        public string TradeVolume { get; set; }

        /// <summary>
        /// 成交金額
        /// </summary>
        public string TradeValue { get; set; }

        /// <summary>
        /// 開盤價
        /// </summary>
        public string OpeningPrice { get; set; }

        /// <summary>
        /// 最高價
        /// </summary>
        public string HighestPrice { get; set; }

        /// <summary>
        /// 最低價
        /// </summary>
        public string LowestPrice { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public string ClosingPrice { get; set; }

        /// <summary>
        /// 漲跌價差
        /// </summary>
        public string Change { get; set; }

        /// <summary>
        /// 成交筆數
        /// </summary>
        public string Transaction { get; set; }
    }
}