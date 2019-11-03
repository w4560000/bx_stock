using System;
using System.ComponentModel.DataAnnotations;

namespace BX_Stock.Models.Dto.TwseDto
{
    /// <summary>
    /// 個股查詢詳細資訊Dto
    /// </summary>
    public class StockDayDetailDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 成交股數
        /// </summary>
        [Display(Name = "成交股數")]
        public int TradeVolume { get; set; }

        /// <summary>
        /// 成交金額
        /// </summary>
        [Display(Name = "成交金額")]
        public double TradeValue { get; set; }

        /// <summary>
        /// 開盤價
        /// </summary>
        [Display(Name = "開盤價")]
        public double OpeningPrice { get; set; }

        /// <summary>
        /// 最高價
        /// </summary>
        [Display(Name = "最高價")]
        public double HighestPrice { get; set; }

        /// <summary>
        /// 最低價
        /// </summary>
        [Display(Name = "最低價")]
        public double LowestPrice { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        [Display(Name = "收盤價")]
        public double ClosingPrice { get; set; }

        /// <summary>
        /// 漲跌價差
        /// </summary>
        [Display(Name = "漲跌價差")]
        public double Change { get; set; }

        /// <summary>
        /// 成交筆數
        /// </summary>
        [Display(Name = "成交筆數")]
        public int Transaction { get; set; }
    }
}