using System;
using System.Collections.Generic;

namespace BX_Stock.Models.Entity
{
    public partial class TblStock
    {
        public DateTime? StockDate { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public long? Volume { get; set; }
        public decimal? Adjusted { get; set; }
    }
}
