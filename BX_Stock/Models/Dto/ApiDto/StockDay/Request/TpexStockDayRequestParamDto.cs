namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 櫃買中心StockDay API requestParamDto
    /// </summary>
    public class TpexStockDayRequestParamDto
    {
        /// <summary>
        /// 回傳格式
        /// </summary>
        public string Response { get; set; } = "json";

        /// <summary>
        /// 日期 (ex 2025/12/01)
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string Code { set; get; }
    }
}