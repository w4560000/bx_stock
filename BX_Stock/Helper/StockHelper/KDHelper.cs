using BX_Stock.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Helper
{
    /// <summary>
    /// KDHelper
    /// </summary>
    public static class KDHelper
    {
        /// <summary>
        /// 計算當前KD (日 or 週 or 月)
        /// 計算清單最後一筆資料的KD
        /// </summary>
        /// <param name="stockDayList">個股資料</param>
        /// <param name="n">預設天數</param>
        public static IStockEntity CalcCurrentKD(this List<IStockEntity> stockList, int n = 9)
        {
            if (stockList.Count != n)
                throw new Exception($"個股資料清單有誤, stockListCount={stockList.Count}, n={n}.");

            decimal beforeK = stockList[stockList.Count - 1 - 1].K;
            decimal beforeD = stockList[stockList.Count - 1 - 1].D;
            decimal closeNow = stockList.Last().ClosingPrice;

            (decimal highestPrice, decimal lowestPrice) = GethighestPriceAndLowestPrice(stockList);
            stockList.Last().K = Calc_K(closeNow, lowestPrice, highestPrice, beforeK);
            stockList.Last().D = Calc_D(beforeD, stockList.Last().K);

            return stockList.Last();
        }

        /// <summary>
        /// 計算KD
        /// </summary>
        /// <param name="stockDayList">個股資料</param>
        /// <param name="n">預設天數</param>
        public static List<IStockEntity> CalcAllKD(this List<IStockEntity> stockDayList, int n = 9, bool isInit = true)
        {
            for (int i = 0; i < stockDayList.Count; i++)
            {
                // 第一天 預設都為50
                if (i == 0)
                {
                    if (isInit)
                    {
                        stockDayList[i].K = 50;
                        stockDayList[i].D = 50;
                    }
                    continue;
                }

                decimal beforeK = stockDayList[i - 1].K;
                decimal closeNow = stockDayList[i].ClosingPrice;

                List<IStockEntity> recentStockData = stockDayList.GetRange(Math.Max(0, i + 1 - n), i < n ? i + 1 : n);
                (decimal highestPrice, decimal lowestPrice) = GethighestPriceAndLowestPrice(recentStockData);
                stockDayList[i].K = Calc_K(closeNow, lowestPrice, highestPrice, beforeK);
                stockDayList[i].D = Calc_D(stockDayList[i - 1].D, stockDayList[i].K);
            }

            return stockDayList;
        }

        /*
                        第n天收盤價-最近n天內最低價
             RSV ＝────────────────────────────────── × 100
                      最近n天內最高價-最近n天內最低價

             計算出RSV之後，再來計算K值與D值。
             當日K值(%K)= 2/3 前一日 K值 + 1/3 RSV
             當日D值(%D)= 2/3 前一日 D值 + 1/3 當日K值
        */

        /// <summary>
        /// 計算K值
        /// </summary>
        /// <param name="closeNow">當天收盤價</param>
        /// <param name="lowestPrice">最低價</param>
        /// <param name="highestPrice">最高價</param>
        /// <param name="yesterdayK">昨天K值</param>
        /// <returns>K值</returns>
        private static decimal Calc_K(decimal closeNow, decimal lowestPrice, decimal highestPrice, decimal yesterdayK)
        {
            // 防止相減為0時相除報錯
            decimal RSV = (closeNow - lowestPrice == 0 || highestPrice - lowestPrice == 0) ? 0 : (closeNow - lowestPrice) / (highestPrice - lowestPrice);

            return (yesterdayK * 2 / 3) + RSV * 100 / 3;
        }

        /// <summary>
        /// 計算D值
        /// </summary>
        /// <param name="yesterdayD">昨日D值</param>
        /// <param name="todayK">今日K值</param>
        /// <returns></returns>
        private static decimal Calc_D(decimal yesterdayD, decimal todayK)
        {
            return (yesterdayD * 2 / 3) + (todayK / 3);
        }

        /// <summary>
        /// 取得最高價和最低價
        /// </summary>
        /// <param name="n">近幾日</param>
        /// <param name="currentIndex">當前Index</param>
        /// <param name="stockDayList">StockDayList</param>
        /// <returns>最高價和最低價</returns>
        private static (decimal, decimal) GethighestPriceAndLowestPrice(List<IStockEntity> stockDayList)
        {
            return (stockDayList.Max(m => m.HighestPrice), stockDayList.Min(m => m.LowestPrice));
        }
    }
}