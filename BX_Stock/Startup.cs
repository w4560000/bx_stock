using BX_Stock.Models;
using BX_Stock.Models.Entity;
using BX_Stock.Repository;
using BX_Stock.Service;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BX_Stock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddTransient<IDbConnection>(sp =>
                new SqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"))
            );

            // 註冊AutoMapper
            services.SetAutoMapper();

            // DI
            services.AddSingleton<ITaskProvider, TaskProvider>();
            services.AddSingleton<StockRepository>();
            services.AddScoped<IBaseApiService, BaseApiService>();
            services.AddScoped<IWebCrawlerService, WebCrawlerService>();
            services.AddScoped<ITwseAPIService, TwseAPIService>();
            services.AddScoped<ITpexAPIService, TpexAPIService>();
            services.AddScoped<IStockService, StockService>();

            // 註冊Hangfire排程
            services.SettingHangfire(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();

            // 設定Hangfire排程方法
            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1 });
            HangfireSetting.SettingHangfire(serviceProvider);

            // 設定hangfire儀表板
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                StatsPollingInterval = 10000,
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}