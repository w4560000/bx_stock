namespace BX_Stock.Models.Dto
{
    /// <summary>
    /// 櫃買中心StockDay API requestParamDto
    /// </summary>
    public class TpexStockDayRequestParamDto
    {
        /// <summary>
        /// 語系
        /// </summary>
        public string L { get; set; } = "zh-tw";

        /// <summary>
        /// 日期 (ex 108/11)
        /// </summary>
        public string D { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string Stkno { set; get; }
    }
}