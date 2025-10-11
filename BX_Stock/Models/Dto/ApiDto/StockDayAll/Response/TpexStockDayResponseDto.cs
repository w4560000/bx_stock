using Newtonsoft.Json;
using System.Collections.Generic;

namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 上櫃個股回傳查詢資訊Dto
    /// </summary>
    public class TpexStockDayAllResponseDto
    {
        /// <summary>
        /// 日期 (ex 1141009)
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string SecuritiesCompanyCode { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 成交股數
        /// </summary>
        public string TradingShares { get; set; }

        /// <summary>
        /// 成交金額
        /// </summary>
        public string TransactionAmount { get; set; }

        /// <summary>
        /// 開盤價
        /// </summary>
        public string Open { get; set; }

        /// <summary>
        /// 最高價
        /// </summary>
        public string High { get; set; }

        /// <summary>
        /// 最低價
        /// </summary>
        public string Low { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public string Close { get; set; }

        /// <summary>
        /// 漲跌價差 (ex: "+0.50")
        /// </summary>
        public string Change { get; set; }

        /// <summary>
        /// 成交筆數
        /// </summary>
        public string TransactionNumber { get; set; }
    }
}