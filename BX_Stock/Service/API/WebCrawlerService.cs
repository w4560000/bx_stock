using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using BX_Stock.Models.Dto;
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
        /// 爬取全部股票代號
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>全部股票代號</returns>
        public async Task<List<StockNoDto>> GetAllStockNoAsync(string url)
        {
            var htmlContent = await this.GetAsync(url);
            var document = _parser.ParseDocument(htmlContent);

            var trCount = document.QuerySelectorAll("tr");

            List<StockNoDto> stockNoDto = new List<StockNoDto>();

            foreach (IElement element in trCount)
            {
                IElement tdB = element.QuerySelector("td > b");

                // 過濾網頁 Td的title 只抓個股資料
                if (tdB == null)
                {
                    IElement tdFirstChild = element.QuerySelector("td:nth-child(1)");

                    if (!tdFirstChild.InnerHtml.Contains("有價證券代號及名稱"))
                    {
                        string[] data = tdFirstChild.InnerHtml.Split("　");
                        stockNoDto.Add(new StockNoDto() { StockNo = data[0], StockName = data[1] });
                    }
                }

                // 只抓上市個股 不抓權證
                if (tdB != null && tdB.InnerHtml.Contains("上市認購(售)權證"))
                {
                    break;
                }
            }

            return stockNoDto;
        }
    }
}