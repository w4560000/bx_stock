using BX_Stock.Service;
using System;

namespace BX_Stock.Models.Entity
{
    public class MovingAverage
    {
        public int StockNo { get; set; }
        public DateTime Date { get; set; }
        public decimal MA5 { get; set; }
        public decimal MA10 { get; set; }
        public decimal MA20 { get; set; }
        public decimal MA30 { get; set; }
        public decimal MA60 { get; set; }
        public decimal MA180 { get; set; }
        public decimal MA365 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
