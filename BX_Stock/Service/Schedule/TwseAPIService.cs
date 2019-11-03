using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Dto.TwseDto;
using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Service
{
    public class TwseAPIService : ITwseAPIService
    {
        private readonly IBaseApiService BaseApiService;

        private readonly IWebCrawlerService WebCrawlerService;

        private readonly StockContext StockContext;

        private readonly IMapper Mapper;

        public TwseAPIService(
            IBaseApiService baseApiService,
            IWebCrawlerService webCrawlerService,
            StockContext stockContext,
            IMapper mapper)
        {
            this.BaseApiService = baseApiService;
            this.WebCrawlerService = webCrawlerService;
            this.StockContext = stockContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        public StockDayDto GetStockData(string stockNo, DateTime date)
        {
            StockDayRequestParamDto test = new StockDayRequestParamDto()
            {
                Date = date.ToString("yyyyMMdd"),  //"20190901",
                StockNo = stockNo
            };

            // 連續串證交所API 會被鎖IP，每隔三秒串一次
            System.Threading.Thread.Sleep(3000);

            StockDayResponseDto stockData = this.BaseApiService
                .Get<StockDayResponseDto, StockDayRequestParamDto>(TwseApiUrl.StockDay, test);

            StockDayDto result = this.Mapper.Map<StockDayDto>(stockData);

            return result;
        }

        /// <summary>
        /// 取得所有股票代號
        /// </summary>
        /// <returns>股票代號清單</returns>
        public List<StockNoDto> GetAllStockNo()
        {
            return this.WebCrawlerService.GetAllStockNoAsync(TwseApiUrl.GetAllStockNo + StrMode.market.GetHashCode())
                                         .GetAwaiter()
                                         .GetResult();
        }

        public void Test3037()
        {
            string stockNo = this.StockContext.Stock.Where(w => w.StockNo == "3037").Select(s => s.StockNo).FirstOrDefault();

            DateTime twseDataStartMonth = DateTime.Parse("2010-01-01");
            DateTime currentMonth = DateTime.Parse(DateTime.Parse("2010-05-01").ToString("yyyy-MM-dd"));

            List<StockDay> result = new List<StockDay>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(currentMonth))
            {
                StockDayDto stockDayDto = this.GetStockData(stockNo, date);

                result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));

            }

            result.ForEach(f => f.StockNo = stockNo);
            this.StockContext.AddRange(result);
            this.StockContext.SaveChanges();

        }
    }
}