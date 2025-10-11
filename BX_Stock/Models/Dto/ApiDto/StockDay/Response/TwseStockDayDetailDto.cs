using System;
using System.ComponentModel.DataAnnotations;

namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 上櫃個股查詢詳細資訊Dto
    /// </summary>
    public class TpexStockDayDetailDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日 期")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 成交張數
        /// </summary>
        [Display(Name = "成交張數")]
        public long TradeVolume { get; set; }

        /// <summary>
        /// 成交仟元
        /// </summary>
        [Display(Name = "成交仟元")]
        public long TradeValue { get; set; }

        /// <summary>
        /// 開盤
        /// </summary>
        [Display(Name = "開盤")]
        public double OpeningPrice { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        [Display(Name = "最高")]
        public double HighestPrice { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        [Display(Name = "最低")]
        public double LowestPrice { get; set; }

        /// <summary>
        /// 收盤
        /// </summary>
        [Display(Name = "收盤")]
        public double ClosingPrice { get; set; }

        /// <summary>
        /// 漲跌
        /// </summary>
        [Display(Name = "漲跌")]
        public double Change { get; set; }

        /// <summary>
        /// 筆數
        /// </summary>
        [Display(Name = "筆數")]
        public int Transaction { get; set; }
    }
}