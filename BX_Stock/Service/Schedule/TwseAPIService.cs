using AutoMapper;
using BX_Stock.Models.Dto.TwseDto;

namespace BX_Stock.Service
{
    public class TwseAPIService : ITwseAPIService
    {
        private readonly IBaseApiService BaseApiService;

        private readonly IWebCrawlerService WebCrawlerService;

        private readonly IMapper Mapper;

        public TwseAPIService(
            IBaseApiService baseApiService,
            IWebCrawlerService webCrawlerService,
            IMapper mapper)
        {
            this.BaseApiService = baseApiService;
            this.WebCrawlerService = webCrawlerService;
            this.Mapper = mapper;
        }

        public StockDayResponseDto GetStockData()
        {
            StockDayRequestParamDto test = new StockDayRequestParamDto()
            {
                Date = "20190901",
                StockNo = "3037"
            };

            StockDayResponseDto result = this.BaseApiService
                .Get<StockDayResponseDto, StockDayRequestParamDto>(TwseApiUrl.StockDay, test);

            StockDayDto q = this.Mapper.Map<StockDayDto>(result);

            var t = this.WebCrawlerService.GetDataAsync(TwseApiUrl.GetAllStockNo + StrMode.market.GetHashCode());


            return result;
        }
    }
}