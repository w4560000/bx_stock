namespace BX_Stock.Models.Dto.StockDto
{
    public class QueryStockDto
    {
        public int StockNo { get; set; } = -1;
        public bool? IsListed { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
