using BX_Stock.Models.Entity;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BX_Stock.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITwseAPIService TwseAPIService;

        private readonly ITpexAPIService TpexAPIService;

        private readonly IStockService StockService;

        public TestController(
            ITwseAPIService twseAPIService,
            IStockService stockService,
            ITpexAPIService tpexAPIService)
        {
            this.TwseAPIService = twseAPIService;
            this.StockService = stockService;
            this.TpexAPIService = tpexAPIService;
        }

        [HttpPost]
        public async Task<string> ProcessNewStock_Schedule1()
        {
            try
            {
                var date = DateTime.Now;

                // 每日排程 更新個股
                await StockService.ProcessNewStock_Schedule1(date);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";

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
        }

        [HttpPost]
        public async Task<string> ProcessStockHistoryData()
        {
            try
            {
                await StockService.ProcessStockHistoryData();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        [HttpPost]
        public async Task<string> ProcessStockDay(DateTime date)
        {
            try
            {
                await StockService.ProcessStockDay(date);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }
    }
}