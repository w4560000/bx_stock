namespace BX_Stock.Models.Entity
{
    public partial class Stock
    {
        public int StockNo { get; set; }
        public string StockName { get; set; }
        public bool IsListed { get; set; }
        public bool IsEnabled { get; set; }
    }
}