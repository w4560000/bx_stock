using System;
using System.Collections.Generic;

namespace BX_Stock.Models.Entity
{
    public partial class StockDay
    {
        public string StockNo { get; set; }
        public DateTime Date { get; set; }
        public long TradeValue { get; set; }
        public decimal OpeningPrice { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal Change { get; set; }
        public int Transaction { get; set; }
        public decimal? K { get; set; }
        public decimal? D { get; set; }
        public decimal? Dif { get; set; }
        public decimal? Dea { get; set; }
        public decimal? Osc { get; set; }
    }
}
