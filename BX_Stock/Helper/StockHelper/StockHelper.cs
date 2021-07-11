using BX_Stock.Models.Entity;
using BX_Stock.Service;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Helper
{
    public static class StockHelper
    {
        /// <summary>
        /// Convent StockDay Of List to StockWeek or StockMonth
        /// </summary>
        /// <typeparam name="T">Convert Type (StockWeek or StockMonth)</typeparam>
        /// <param name="stockDayList">stockDayList</param>
        /// <returns>StockWeek or StockMonth</returns>
        public static T GetStockWeekOrMonthData<T>(this List<StockDay> stockDayList) where T : IStockEntity, new()
        {
            return new T()
            {
                StockNo = stockDayList.First().StockNo,
                Date = stockDayList.First().Date,
                TradeValue = stockDayList.Sum(x => x.TradeValue),
                OpeningPrice = stockDayList.First().OpeningPrice,
                HighestPrice = stockDayList.Max(x => x.HighestPrice),
                LowestPrice = stockDayList.Min(x => x.LowestPrice),
                ClosingPrice = stockDayList.Last().ClosingPrice,
                Change = stockDayList.First().OpeningPrice - stockDayList.Last().ClosingPrice,
                Transaction = stockDayList.Sum(x => x.Transaction)
            };
        }

        /// <summary>
        /// 將依照週期或月份分類好的個股資訊轉成單月or單週個股資訊
        /// </summary>
        /// <typeparam name="T">要轉換的個股週期類型(月或週)</typeparam>
        /// <param name="stockDataList">分類成月或週的單日個股資訊</param>
        /// <returns>單月or單週個股資訊</returns>
        public static List<IStockEntity> AsStockWeekOrMonth<T>(this List<List<StockDay>> stockDataList) where T : IStockEntity, new()
        {
            List<IStockEntity> result = new List<IStockEntity>();
            stockDataList.ForEach(x => result.Add(x.GetStockWeekOrMonthData<T>()));

            return result;
        }
    }
}