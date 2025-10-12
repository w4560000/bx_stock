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

        /// <summary>
        /// 每日排程更新個股股號
        /// </summary>
        [HttpPost]
        public async Task<string> 每日排程更新個股股號()
        {
            try
            {
                var date = DateTime.Now;

                await StockService.每日排程更新個股股號(date);
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

        /// <summary>
        /// 每日排程撈最新上市個股資訊
        /// </summary>
        [HttpPost]
        public async Task<string> 每日排程撈最新上市個股資訊()
        {
            try
            {
                await StockService.每日排程撈最新上市個股資訊();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 每日排程撈最新上櫃個股資訊
        /// </summary>
        [HttpPost]
        public async Task<string> 每日排程撈最新上櫃個股資訊()
        {
            try
            {
                await StockService.每日排程撈最新上櫃個股資訊();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 重撈上市個股歷史資訊
        /// </summary>
        [HttpPost]
        public async Task<string> 重撈上市個股歷史資訊()
        {
            try
            {
                await StockService.重撈上市個股歷史資訊();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 重撈上櫃個股歷史資訊
        /// </summary>
        [HttpPost]
        public async Task<string> 重撈上櫃個股歷史資訊()
        {
            try
            {
                await StockService.重撈上櫃個股歷史資訊();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 重撈上市個股日資訊
        /// </summary>
        /// <param name="date">日期</param>
        [HttpPost]
        public async Task<string> 重撈上市個股日資訊(DateTime date)
        {
            try
            {
                await StockService.重撈上市個股日資訊(date);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 重撈上櫃個股日資訊
        /// </summary>
        /// <param name="date">日期</param>
        [HttpPost]
        public async Task<string> 重撈上櫃個股日資訊(DateTime date)
        {
            try
            {
                await StockService.重撈上櫃個股日資訊(date);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 計算移動平均線日資訊
        /// </summary>
        /// <param name="date">日期</param>
        [HttpPost]
        public async Task<string> 計算移動平均線日資訊(DateTime date)
        {
            try
            {
                await StockService.計算移動平均線日資訊(date);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// 計算移動平均線歷史指標
        /// </summary>
        [HttpPost]
        public async Task<string> 計算移動平均線歷史指標()
        {
            try
            {
                await StockService.計算移動平均線歷史指標();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }
    }
}