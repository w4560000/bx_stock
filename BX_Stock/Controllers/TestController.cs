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
            return this.View();
        }
    }
}