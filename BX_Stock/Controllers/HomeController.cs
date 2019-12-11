using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BX_Stock.Models;
using Hangfire;
using BX_Stock.Service;

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
