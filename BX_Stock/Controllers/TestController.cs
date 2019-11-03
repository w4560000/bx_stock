using BX_Stock.Helper;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;

namespace BX_Stock.Controllers
{
    public class TestController : Controller
    {
        private readonly ITwseAPIService TwseAPIService;

        public TestController(ITwseAPIService twseAPIService)
        {
            this.TwseAPIService = twseAPIService;
        }

        public IActionResult Index()
        {
            //var a = this.TwseAPIService.GetStockData();

            this.TwseAPIService.Test3037();

            return this.View();
        }
    }
}