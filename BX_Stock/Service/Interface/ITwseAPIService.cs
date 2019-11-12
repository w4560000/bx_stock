using BX_Stock.Models.Dto.TwseDto;
using System;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    public interface ITwseAPIService
    {
        /// <summary>
        /// 第一次 新增個股資訊
        /// </summary>
        void FirstInsertStockNo();
        void TestInsert1515();
    }
}