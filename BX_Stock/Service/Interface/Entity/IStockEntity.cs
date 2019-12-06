using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public interface IStockEntity
    {
        int StockNo { get; set; }
        DateTime Date { get; set; }
        long TradeValue { get; set; }
        decimal OpeningPrice { get; set; }
        decimal HighestPrice { get; set; }
        decimal LowestPrice { get; set; }
        decimal ClosingPrice { get; set; }
        decimal Change { get; set; }
        int Transaction { get; set; }
        decimal K { get; set; }
        decimal D { get; set; }
        decimal Ema12 { get; set; }
        decimal Ema26 { get; set; }
        decimal Dif { get; set; }
        decimal Dea { get; set; }
        decimal Osc { get; set; }
    }
}
