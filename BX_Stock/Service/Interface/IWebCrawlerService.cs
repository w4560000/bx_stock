using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public interface IWebCrawlerService
    {
        Task GetDataAsync(string url);
    }
}
