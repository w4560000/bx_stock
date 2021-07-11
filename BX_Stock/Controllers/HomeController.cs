using BX_Stock.Models;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BX_Stock.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;

        private readonly ITwseAPIService TSECAPIService;

        public HomeController(
            ILogger<HomeController> logger,
            ITwseAPIService tSECAPIService)
        {
            this.Logger = logger;
            this.TSECAPIService = tSECAPIService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}