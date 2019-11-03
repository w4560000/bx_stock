using BX_Stock.Models.Dto.TwseDto;
using System;

namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        /// <summary>
        /// 取得個股單月資訊
        /// </summary>
        /// <returns>個股單月資訊</returns>
        StockDayDto GetStockData(string stockNo, DateTime date);

        void Test3037();
    }
}