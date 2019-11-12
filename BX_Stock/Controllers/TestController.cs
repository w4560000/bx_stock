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
            this.TwseAPIService.TestInsert1515();

            return this.View();
        }
    }
}