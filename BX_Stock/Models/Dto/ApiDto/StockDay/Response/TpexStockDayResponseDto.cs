using System.Collections.Generic;

namespace BX_Stock.Models.Dto
{
    public class TpexStockDayResponseDto
    {
        /// <summary>
        /// 回傳狀態
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股代號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public List<TpexStockDayResponseDetailDto> Tables { get; set; }
    }

    public class TpexStockDayResponseDetailDto : IStockDayDetailDto
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 查詢日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 個股資訊
        /// </summary>
        public List<List<string>> Data { get; set; }

        /// <summary>
        /// 個股資訊欄位
        /// </summary>
        public List<string> Fields { get; set; }
    }
}