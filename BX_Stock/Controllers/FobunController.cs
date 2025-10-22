using BX_Stock.Models.Dto;
using BX_Stock.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BX_Stock.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FobunController : ControllerBase
    {
        private readonly ITwseAPIService _twseAPIService;

        private readonly ITpexAPIService _tpexAPIService;

        private readonly StockService _stockService;

        public FobunController(
            ITwseAPIService twseAPIService,
            StockService stockService,
            ITpexAPIService tpexAPIService)
        {
            this._twseAPIService = twseAPIService;
            this._stockService = stockService;
            this._tpexAPIService = tpexAPIService;
        }

        /// <summary>
        /// 撈取歷史個股分K資料
        /// </summary>
        [HttpPost]
        public async Task<string> 撈取歷史個股分K資料()
        {
            try
            {
                await _stockService.撈取歷史個股分K資料();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "OK";
        }
    }
}