using AutoMapper;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Dto.TwseDto;
using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// 第一次建立個股資料
        /// </summary>
        public void FirstInsertStockNo()
        {
            List<Stock> AllStockNo = new List<Stock>();

            // 爬取上市股票
            AllStockNo.AddRange(this.WebCrawlerService.GetAllListedStockNoAsync().GetAwaiter().GetResult());

            // 爬取上櫃股票
            AllStockNo.AddRange(this.WebCrawlerService.GetAllCabinetStockNoAsync().GetAwaiter().GetResult());

            this.StockContext.AddRange(AllStockNo);
            this.StockContext.SaveChanges();
        }

        /// <summary>
        /// 每日排程 撈取現有股號
        /// 若撈取的資料與現有資料庫股號有差異 
        /// 則移除下架的個股與相關資訊，並新增上架的個股與相關資訊
        /// </summary>
        /// <returns>股票代號清單</returns>
        public void GetAllStockNo()
        {
            List<Stock> GetAllStockNo = this.WebCrawlerService
                                            .GetAllListedStockNoAsync()
                                            .GetAwaiter()
                                            .GetResult();
        }

        public void TestInsert1515()
        {
            this.GetStockHistoryData("1515");
        }

        /// <summary>
        /// 新增個股歷史資料
        /// </summary>
        /// <param name="stockNo">要新增的個股</param>
        /// <param name="startMonth">查詢起始時間</param>
        /// <param name="endMonth">查詢結束時間</param>
        public void GetStockHistoryData(string stockNo,string startMonth = "2010-01", string endMonth = "")
        {
            DateTime twseDataStartMonth = DateTime.Parse(startMonth);
            DateTime currentMonth = string.IsNullOrEmpty(endMonth) ?  DateTime.Now : DateTime.Parse(endMonth);
            List<StockDay> result = new List<StockDay>();

            foreach (DateTime date in twseDataStartMonth.EachMonthTo(currentMonth))
            {
                
                StockDayDto stockDayDto = this.GetStockData(stockNo, date);

                result.AddRange(this.Mapper.Map<List<StockDay>>(stockDayDto.Data));
            }

            result.ForEach(f => f.StockNo = stockNo);

            // 計算KD
            result.CalcKD();

            this.StockContext.AddRange(result);
            this.StockContext.SaveChanges();
        }

        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        private StockDayDto GetStockData(string stockNo, DateTime date)
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

    }
}