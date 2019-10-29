namespace BX_Stock.Models.Dto.TwseDto
{
    /// <summary>
    /// 個股查詢Request參數
    /// </summary>
    public class StockDayRequestParamDto
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