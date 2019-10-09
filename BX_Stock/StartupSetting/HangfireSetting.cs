using BX_Stock.Service;
using Hangfire;
using Hangfire.SQLite;
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
        public static void SettingHangfire(this IServiceCollection services)
        {
            string SqliteSource = "Filename=./BXStockHangfire.db;";
            services.AddHangfire(configuration => configuration.UseSQLiteStorage(SqliteSource));
        }

        /// <summary>
        /// 載入
        /// </summary>
        /// <param name="services">服務</param>
        public static void SettingHangfire(IServiceProvider serviceProvider)
        {
            //GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(serviceProvider));

            TaskProvider taskProvider = (TaskProvider)serviceProvider.GetService(typeof(ITaskProvider));

            taskProvider.RecurringTask<ITSECAPIService>("測試", x => x.Test(), Cron.Daily(0));
        }
    }
}