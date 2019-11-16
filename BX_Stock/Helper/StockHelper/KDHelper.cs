using BX_Stock.Models.Entity;
using BX_Stock.Service;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BX_Stock.Helper
{
    /// <summary>
    /// KDHelper
    /// </summary>
    public static class KDHelper
    {
        /// <summary>
        /// 計算KD
        /// </summary>
        /// <param name="stockDayList">個股資料</param>
        /// <param name="n">預設天數</param>
        public static List<IStockEntity> CalcKD(this List<IStockEntity> stockDayList, int n = 9)
        {
            /*
                        第n天收盤價-最近n天內最低價
             RSV ＝────────────────────────────────── × 100
                      最近n天內最高價-最近n天內最低價

             計算出RSV之後，再來計算K值與D值。
             當日K值(%K)= 2/3 前一日 K值 + 1/3 RSV
             當日D值(%D)= 2/3 前一日 D值 + 1/3 當日K值
             */

            decimal closeNow = 0;
            decimal lowestPrice = 0;
            decimal highestPrice = 0;
            decimal beforeK = 0;

            // 計算K
            static decimal Calc_K(decimal closeNow, decimal lowestPrice, decimal highestPrice, decimal yesterdayK)
            {
                return (yesterdayK * 2 / 3) + (closeNow - lowestPrice) / (highestPrice - lowestPrice) * 100 / 3;
            }

            // 計算D
            static decimal Calc_D(decimal yesterdayD, decimal todayK)
            {
                return (yesterdayD * 2 / 3) + (todayK / 3);
            }

            // 取得最近n天 最高價與最低價
            static (decimal, decimal) GethighestPriceAndLowestPrice(int n, int currentIndex, List<IStockEntity> stockDayList)
            {
                List<IStockEntity> recentStockData = stockDayList.GetRange(Math.Max(0, currentIndex +1 - n), currentIndex < 9 ? currentIndex + 1 : 9);
                recentStockData.Sort((x, y) => { return x.LowestPrice.CompareTo(y.LowestPrice); });
                decimal lowestPrice = recentStockData[0].LowestPrice;

                recentStockData.Sort((x, y) => { return -x.HighestPrice.CompareTo(y.HighestPrice); });
                decimal highestPrice = recentStockData[0].HighestPrice;

                return (lowestPrice, highestPrice);
            }

            for (int i = 0; i < stockDayList.Count; i++)
            {
                // 第一天 預設都為50
                if (i == 0)
                {
                    stockDayList[i].K = 50;
                    stockDayList[i].D = 50;
                    continue;
                }

                beforeK = stockDayList[i - 1].K;
                closeNow = stockDayList[i].ClosingPrice;
                (lowestPrice, highestPrice) = GethighestPriceAndLowestPrice(n, i, stockDayList);
                stockDayList[i].K = Calc_K(closeNow, lowestPrice, highestPrice, beforeK);
                stockDayList[i].D = Calc_D(stockDayList[i - 1].D, stockDayList[i].K);
            }

            return stockDayList;
        }
    }
}