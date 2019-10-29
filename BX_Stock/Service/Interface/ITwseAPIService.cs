using BX_Stock.Models.Dto.TwseDto;

namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        public StockDayResponseDto GetStockData();
    }
}