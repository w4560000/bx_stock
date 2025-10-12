using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Helper.StockHelper
{
    public static class MovingAverageHelper
    {
        /// <summary>
        /// 計算移動平均線
        /// </summary>
        /// <param name="stockList">個股資料</param>
        /// <param name="movingAverageList">移動平均線清單</param>
        /// <param name="n">天數</param>
        public static void Calc(List<StockDay> stockList, List<MovingAverage> movingAverageList, int n)
        {
            if (stockList == null || stockList.Count < n)
                return;

            // 依日期排序
            stockList = stockList.OrderBy(o => o.Date).ToList();
            var maDict = movingAverageList.ToDictionary(m => m.Date, m => m);

            for (int i = n - 1; i < stockList.Count; i++)
            {
                var date = stockList[i].Date;
                if (!maDict.TryGetValue(date, out var movingAverageData))
                    continue;

                // 取出連續 n 天範圍
                decimal avg = stockList
                    .Skip(i - n + 1)
                    .Take(n)
                    .Average(x => x.ClosingPrice);

                // 根據天數設定對應欄位
                switch (n)
                {
                    case 5: movingAverageData.MA5 = avg; break;
                    case 10: movingAverageData.MA10 = avg; break;
                    case 20: movingAverageData.MA20 = avg; break;
                    case 30: movingAverageData.MA30 = avg; break;
                    case 60: movingAverageData.MA60 = avg; break;
                    case 180: movingAverageData.MA180 = avg; break;
                    case 365: movingAverageData.MA365 = avg; break;
                }
            }
        }
    }
}