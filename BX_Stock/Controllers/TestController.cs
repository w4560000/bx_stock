using BX_Stock.Helper;
using BX_Stock.Models.Entity;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BX_Stock.Controllers
{
    public class TestController : Controller
    {
        private readonly ITwseAPIService TwseAPIService;

        private readonly IStockService StockService;

        private readonly StockContext StockContext;
        public TestController(ITwseAPIService twseAPIService, IStockService stockService, StockContext stockContext)
        {
            this.TwseAPIService = twseAPIService;
            this.StockService = stockService;
            this.StockContext = stockContext;
        }

        public IActionResult Index()
        {
            //this.ViewData["Title"] = this.StockContext.Set<Stock>().FirstOrDefault().StockName;
            //this.TwseAPIService.ProcessStockScheduleFirst(1204, 1210);

            return this.View();
        }
    }
}