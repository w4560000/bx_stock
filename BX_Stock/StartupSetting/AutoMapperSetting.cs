using AutoMapper;
using BX_Stock.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BX_Stock
{
    /// <summary>
    /// 註冊AutoMapper
    /// </summary>
    public static class AutoMapperSetting
    {
        /// <summary>
        /// 註冊AutoMapper方法
        /// </summary>
        public static void SetAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Startup));
        }
    }
}