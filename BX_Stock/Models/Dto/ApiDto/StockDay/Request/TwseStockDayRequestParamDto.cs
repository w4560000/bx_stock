namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 證交所StockDay API requestParamDto
    /// </summary>
    public class TwseStockDayRequestParamDto
    {
        /// <summary>
        /// 回傳格式
        /// </summary>
        public string Response { get; set; } = "json";

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 查詢股號
        /// </summary>
        public string StockNo { get; set; }
    }
}