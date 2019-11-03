using BX_Stock.Models.Entity;
using BX_Stock.Service;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
            services.AddDbContext<StockContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            // ���UAutoMapper
            services.SetAutoMapper();

            // DI
            services.AddSingleton<ITaskProvider, TaskProvider>();
            services.AddScoped<IBaseApiService, BaseApiService>();
            services.AddScoped<IWebCrawlerService, WebCrawlerService>();
            services.AddScoped<ITwseAPIService, TwseAPIService>();

            // ���UHangfire�Ƶ{
            services.SettingHangfire();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // �]�wHangfire�Ƶ{��k
            app.UseHangfireServer();
            HangfireSetting.SettingHangfire(serviceProvider);

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // �]�whangfire����O
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                StatsPollingInterval = 10000,
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
            });
        }
    }
}