using BX_Stock.Helper;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;

namespace BX_Stock.Controllers
{
    public class TestController : Controller
    {
        private readonly ITwseAPIService TwseAPIService;

        private readonly IStockService StockService;

        public TestController(ITwseAPIService twseAPIService, IStockService stockService)
        {
            this.TwseAPIService = twseAPIService;
            this.StockService = stockService;
        }

        public IActionResult Index()
        {
            //this.TwseAPIService.ProcessStockScheduleFirst(1000, 1200);

            return this.View();
        }
    }
}