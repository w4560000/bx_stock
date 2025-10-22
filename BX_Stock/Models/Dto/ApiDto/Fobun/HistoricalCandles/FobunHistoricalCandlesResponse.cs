using BX_Stock.Models.Entity;
using System.Collections.Generic;

namespace BX_Stock.Models.Dto.ApiDto.Fobun.HistoricalCandles
{
    public class FobunHistoricalCandlesResponse
    {
        /// <summary>
        /// 股票代號
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 證券類型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 市場別
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// Ｋ線資料
        /// </summary>
        public List<FobunHistoricalCandlesData> Data { get; set; }
    }
}
