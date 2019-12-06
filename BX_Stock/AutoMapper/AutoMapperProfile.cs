using AutoMapper;
using BX_Stock.Extension;
using BX_Stock.Helper;
using BX_Stock.Models.Dto;
using BX_Stock.Models.Entity;
using System;
using System.Reflection;

namespace BX_Stock.AutoMapper
{
    /// <summary>
    /// AutoMapperProfile
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// mapperProfile
        /// </summary>
        public AutoMapperProfile()
        {
            // twse個股資訊 轉換成dto
            this.CreateMap<TwseStockDayResponseDto, StockDayDto>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Date))
                .ForMember(dest => dest.IsOK, opts => opts.MapFrom(src => src.Stat.Equals("OK")))
                .ForMember(dest => dest.StockNo, opts => opts.MapFrom(src => src.Title.Trim()))
                .ForMember(dest => dest.Data, opts => opts.MapFrom(src => this.ConvertStockDayDetail(src)))
                .ForAllOtherMembers(i => i.Ignore());

            this.CreateMap<StockDayDetailDto, StockDay>();
        }

        /// <summary>
        /// 證交所回傳資料轉為個股Dto
        /// </summary>
        /// <param name="mapFrom">證交所回傳資料</param>
        /// <returns>個股Dto</returns>
        private StockDayDetailDto[] ConvertStockDayDetail(TwseStockDayResponseDto mapFrom)
        {
            StockDayDetailDto[] result = new StockDayDetailDto[mapFrom.Data.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new StockDayDetailDto();

                for (int j = 0; j < mapFrom.Data[i].Count; j++)
                {
                    foreach (PropertyInfo prop in result[i].GetType().GetProperties())
                    {
                        if (prop.GetDisplayName().Equals(mapFrom.Fields[j]))
                        {
                            if (prop.PropertyType == typeof(DateTime) && DateTime.TryParse(mapFrom.Data[i][j].ConvertToADType(), out DateTime dtDate))
                            {
                                prop.SetValue(result[i], dtDate);
                            }
                            else
                            {
                                // 證交所回傳的漲跌價差 若為0 則顯示 "X0.00" 若直接轉decimal會噴錯
                                if (mapFrom.Data[i][j].Contains("X") || mapFrom.Data[i][j].Equals("--"))
                                {
                                    prop.SetValue(result[i], 0);
                                }
                                else
                                {
                                    prop.SetValue(result[i], Convert.ChangeType(mapFrom.Data[i][j].Replace(",", string.Empty), prop.PropertyType));
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 標題特殊處理空白
        /// </summary>
        /// <param name="title">標題</param>
        /// <returns></returns>
        private string ConvertToStockFromTitle(string title)
        {
            return title.Split(' ')[1];
        }
    }
}