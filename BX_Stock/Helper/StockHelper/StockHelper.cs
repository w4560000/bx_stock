using BX_Stock.Models.Entity;
using BX_Stock.Service;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Helper
{
    public static class StockHelper
    {
        /// <summary>
        /// 將依照週期或月份分類好的個股資訊轉成單月or單週個股資訊
        /// </summary>
        /// <typeparam name="T">要轉換的個股週期類型(月或週)</typeparam>
        /// <param name="stockDataList">分類成月或週的單日個股資訊</param>
        /// <returns>單月or單週個股資訊</returns>
        public static List<IStockEntity> AsStockWeekOrMonth<T>(this List<List<StockDay>> stockDataList) where T : IStockEntity, new()
        {
            List<IStockEntity> result = new List<IStockEntity>();
            stockDataList.ForEach(x =>
            {
                result.Add(new T()
                {
                    StockNo = x.First().StockNo,
                    Date = x.First().Date,
                    TradeValue = x.Sum(x => x.TradeValue),
                    OpeningPrice = x.First().OpeningPrice,
                    HighestPrice = x.Max(x => x.HighestPrice),
                    LowestPrice = x.Min(x => x.LowestPrice),
                    ClosingPrice = x.Last().ClosingPrice,
                    Change = x.First().OpeningPrice - x.Last().ClosingPrice,
                    Transaction = x.Sum(x => x.Transaction)
                });
            });

            return result;
        }
    }
}