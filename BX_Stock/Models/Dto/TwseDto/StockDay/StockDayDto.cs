namespace BX_Stock.Models.Dto.TwseDto
{
    public class StockDayDto
    {
        /// <summary>
        /// 回傳狀態
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public StockDayDetailDto[] Data { get; set; }
    }
}