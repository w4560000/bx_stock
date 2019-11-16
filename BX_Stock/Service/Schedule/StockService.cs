using BX_Stock.Extension;
using BX_Stock.Helper;
using BX_Stock.Models.Entity;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BX_Stock.Service
{
    /// <summary>
    /// 股票Service
    /// </summary>
    public class StockService : IStockService
    {
        private readonly StockContext StockContext;

        public StockService(StockContext stockContext)
        {
            this.StockContext = stockContext;
        }

        /// <summary>
        /// 計算週KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        public void ProcessStockWeekKD(string stockNo)
        {
            // 取出個股每日資訊
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo)).ToList();
            List<List<StockDay>> stockDataListOfWeeks = new List<List<StockDay>>();

            // 以年做區隔 一年一年處理
            foreach (List<StockDay> stockDayList in stockData.GroupBy(x => x.Date.Year).Select(s => s.ToList()).ToList())
            {
                // 以週做區隔
                List<List<StockDay>> stockDataOfWeek = stockDayList.GroupBy(x => new GregorianCalendar().GetWeekOfYear(x.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                                                                   .Select(s => s.ToList())
                                                                   .ToList();

                stockDataListOfWeeks.AddRange(stockDataOfWeek);
            }

            // 轉換成Entity並計算週KD
            List<IStockEntity> stockWeek = stockDataListOfWeeks.AsStockWeekOrMonth<StockWeek>().CalcKD();
            this.StockContext.BulkInsert(stockWeek);
        }

        /// <summary>
        /// 計算月KD
        /// </summary>
        /// <param name="stockNo">個股代號</param>
        public void ProcessStockMonthKD(string stockNo)
        {
            // 取出個股每日資訊
            List<StockDay> stockData = this.StockContext.Set<StockDay>().Where(w => w.StockNo.Equals(stockNo)).ToList();
            List<List<StockDay>> stockDataListOfMonths = new List<List<StockDay>>();

            // 以年做區隔 一年一年處理
            foreach (List<StockDay> stockDayList in stockData.GroupBy(x => x.Date.Year).Select(s => s.ToList()).ToList())
            {
                // 以月份做區隔
                List<List<StockDay>> stockDataListOfMonth = stockDayList.GroupBy(x => x.Date.Month)
                                                                        .Select(s => s.ToList())
                                                                        .ToList();

                stockDataListOfMonths.AddRange(stockDataListOfMonth);
            }

            // 轉換成Entity並計算月KD
            List<IStockEntity> stockMonth = stockDataListOfMonths.AsStockWeekOrMonth<StockMonth>().CalcKD();
            this.StockContext.BulkInsert(stockMonth);
        }

        /// <summary>
        /// 刪除個股資訊
        /// </summary>
        /// <param name="deleteStockNoList">欲刪除的個股代號清單</param>
        public void DeleteStockData(List<string> deleteStockNoList)
        {
            List<Stock> deleteStock = this.StockContext.Set<Stock>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockDay> deleteStockDay = this.StockContext.Set<StockDay>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockWeek> deleteStockWeek = this.StockContext.Set<StockWeek>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();
            List<StockMonth> deleteStockMonth = this.StockContext.Set<StockMonth>().Where(w => deleteStockNoList.Contains(w.StockNo)).ToList();


            using var transaction = this.StockContext.Database.BeginTransaction();
            this.StockContext.BulkDelete(deleteStock);
            this.StockContext.BulkDelete(deleteStockDay);
            this.StockContext.BulkDelete(deleteStockWeek);
            this.StockContext.BulkDelete(deleteStockMonth);

            transaction.Commit();
        }
    }
}