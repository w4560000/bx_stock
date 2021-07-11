using BX_Stock.Service;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BX_Stock
{
    /// <summary>
    /// Hangfire排程設定
    /// </summary>
    public static class HangfireSetting
    {
        /// <summary>
        /// 載入
        /// </summary>
        /// <param name="services">服務集合</param>
        public static void SettingHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
        }

        /// <summary>
        /// 載入
        /// </summary>
        /// <param name="services">服務</param>
        public static void SettingHangfire(IServiceProvider serviceProvider)
        {
            //GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(serviceProvider));

            TaskProvider taskProvider = (TaskProvider)serviceProvider.GetService(typeof(ITaskProvider));

            //taskProvider.RecurringTask<IStockService>("更新上市上櫃個股代碼", x => x.ProcessStockSchedule1(), Cron.Daily());

            //taskProvider.RecurringTask<ITwseAPIService>("新增上市個股代碼", x => x.ProcessStockScheduleFirst(1100, 10000), Cron.Monthly());
            //taskProvider.RecurringTask<ITpexAPIService>("新增上櫃個股代碼", x => x.ProcessStockScheduleFirst(1100, 10000), Cron.Monthly());

            //taskProvider.RecurringTask<ITwseAPIService>("測試", x => x.Test1515(), Cron.Monthly());
        }
    }
}