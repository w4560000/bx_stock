using AutoMapper;
using BX_Stock.Extension;
using BX_Stock.Models.Dto.TwseDto;
using System;
using System.Reflection;

namespace BX_Stock.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<StockDayResponseDto, StockDayDto>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Date))
                .ForMember(dest => dest.IsOK, opts => opts.MapFrom(src => src.Stat.Equals("OK")))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Data, opts => opts.MapFrom(src => this.ConvertStockDayDetail(src)))
                .ForAllOtherMembers(i => i.Ignore());
        }

        private StockDayDetailDto[] ConvertStockDayDetail(StockDayResponseDto mapFrom)
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
                            prop.SetValue(result[i], Convert.ChangeType(mapFrom.Data[i][j].Replace(",", string.Empty), prop.PropertyType));
                        }
                    }
                }
            }

            return result;
        }
    }
}