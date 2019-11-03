using AutoMapper;
using BX_Stock.Extension;
using BX_Stock.Models.Dto.TwseDto;
using BX_Stock.Models.Entity;
using System;
using System.Reflection;

namespace BX_Stock.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // twse個股資訊 轉換成dto
            this.CreateMap<StockDayResponseDto, StockDayDto>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Date))
                .ForMember(dest => dest.IsOK, opts => opts.MapFrom(src => src.Stat.Equals("OK")))
                .ForMember(dest => dest.StockNo, opts => opts.MapFrom(src => this.ConvertToStockFromTitle(src.Title)))
                .ForMember(dest => dest.Data, opts => opts.MapFrom(src => this.ConvertStockDayDetail(src)))
                .ForAllOtherMembers(i => i.Ignore());

            this.CreateMap<StockDayDetailDto, StockDay>();
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
                            if (prop.PropertyType == typeof(DateTime) && DateTime.TryParse(mapFrom.Data[i][j], out DateTime dtDate))
                            {
                                prop.SetValue(result[i], dtDate);
                            }
                            else
                            {
                                prop.SetValue(result[i], Convert.ChangeType(mapFrom.Data[i][j].Replace(",", string.Empty), prop.PropertyType));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private string ConvertToStockFromTitle(string title)
        {
            return title.Split(' ')[1];
        }
    }
}