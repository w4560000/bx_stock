using BX_Stock.Models.Entity;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BX_Stock.Controllers
{
    public class TestController : Controller
    {
        private readonly ITwseAPIService TwseAPIService;

        private readonly ITpexAPIService TpexAPIService;

        private readonly IStockService StockService;

        private readonly StockContext StockContext;

        public TestController(
            ITwseAPIService twseAPIService,
            IStockService stockService,
            StockContext stockContext,
            ITpexAPIService tpexAPIService)
        {
            this.TwseAPIService = twseAPIService;
            this.StockService = stockService;
            this.StockContext = stockContext;
            this.TpexAPIService = tpexAPIService;
        }

        public IActionResult Index()
        {
            var date = DateTime.Now;

            // 每日排程 更新個股
            //StockService.ProcessNewStock_Schedule1(date);

            // 每日排程 新增當日個股資料
            //StockService.ProcessTodayStock_Schedule2();

            // 週六排程 新增新個股 所有週KD
            //StockService.CalcNewStockAllWeekKD();

            // 重新撈取上市個股資料
            //this.TwseAPIService.ProcessStockScheduleFirst(1000, 10000);

            // 重新撈取上櫃個股資料
            //this.TpexAPIService.ProcessStockScheduleFirst(1000, 10000);

            //this.TwseAPIService.ProcessStockHistoryData(1101, "2010-01-04", "2021-06");

            // 重新計算所有日KD
            //StockService.CalcCurrentAllDayKD();

            // 重新計算所有週KD
            //StockService.CalcCurrentAllWeekKD();

            // 計算該週 KD
            //StockService.CalcCurrentWeekKD();

            //bool a = this.StockContext.Set<Stock>().Where(w => w.StockNo == 1101).Select(s => s.IsEnabled).FirstOrDefault();
            return this.View();
        }
    }
}