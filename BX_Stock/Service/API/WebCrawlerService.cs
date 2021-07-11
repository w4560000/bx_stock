using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using BX_Stock.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    /// <summary>
    /// 爬蟲Service
    /// </summary>
    public class WebCrawlerService : IWebCrawlerService
    {
        /// <summary>
        /// HtmlParser
        /// </summary>
        private static readonly HtmlParser _parser = new HtmlParser();

        /// <summary>
        /// 非同步 httpWebRequest
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>response</returns>
        private async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(950));
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// 爬取現有上市的股票代號
        /// </summary>
        /// <returns>現有上市的股票代號</returns>
        public async Task<List<Stock>> GetAllListedStockNoAsync()
        {
            var htmlContent = await this.GetAsync(StockApiUrl.GetAllListedStockNo);
            var document = _parser.ParseDocument(htmlContent);

            var trCount = document.QuerySelectorAll("tr");

            List<Stock> stockNoDto = new List<Stock>();
            bool nextRowGetStock = false;

            foreach (IElement element in trCount)
            {
                IElement tdB = element.QuerySelector("td > b");

                // 從股票底下開始爬上市股票
                if (!nextRowGetStock && tdB != null && tdB.InnerHtml.Contains("股票"))
                {
                    nextRowGetStock = true;
                    continue;
                }

                // 過濾網頁 Td的title 只抓個股資料
                if (nextRowGetStock)
                {
                    if (tdB is null)
                    {
                        IElement tdFirstChild = element.QuerySelector("td:nth-child(1)");
                        string[] data = tdFirstChild.InnerHtml.Split("　");
                        stockNoDto.Add(
                            new Stock() 
                            { 
                                StockNo = Convert.ToInt32(data[0].Split(" ")[0]), 
                                StockName = data[1], 
                                IsListed = true, 
                                IsEnabled = true,
                                IsNew = true
                            });
                    }

                    // 爬到上市認購(售)權證那欄 代表上市股票已全部爬完
                    else if (tdB.InnerHtml.Contains("上市認購(售)權證"))
                    {
                        break;
                    }
                }
            }

            return stockNoDto;
        }

        /// <summary>
        /// 爬取現有上櫃的股票代號
        /// </summary>
        /// <returns>現有上櫃的股票代號</returns>
        public async Task<List<Stock>> GetAllCabinetStockNoAsync()
        {
            var htmlContent = await this.GetAsync(StockApiUrl.GetAllCabinetStockNo);
            var document = _parser.ParseDocument(htmlContent);

            var trCount = document.QuerySelectorAll("tr");

            List<Stock> stockNoDto = new List<Stock>();
            bool nextRowGetStock = false;

            foreach (IElement element in trCount)
            {
                IElement tdB = element.QuerySelector("td > b");

                // 從股票底下開始爬上櫃股票
                if (!nextRowGetStock && tdB != null && tdB.InnerHtml.Contains("股票"))
                {
                    nextRowGetStock = true;
                    continue;
                }

                if (nextRowGetStock)
                {
                    IElement tdFirstChild = element.QuerySelector("td:nth-child(1)");

                    if (tdB is null)
                    {
                        string[] data = tdFirstChild.InnerHtml.Split("　");
                        stockNoDto.Add(new Stock() 
                        { 
                            StockNo =  Convert.ToInt32(data[0].Split(" ")[0]),
                            StockName = data[1], 
                            IsListed = false, 
                            IsEnabled = true,
                            IsNew = true
                        });
                    }

                    // 爬到特別股那欄 代表上櫃股票已全部爬完
                    else if (tdB.InnerHtml.Contains("特別股"))
                    {
                        break;
                    }
                }
            }

            return stockNoDto;
        }
    }
}