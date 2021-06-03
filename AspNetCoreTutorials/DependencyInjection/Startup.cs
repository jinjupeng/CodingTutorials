using DependencyInjection.TestLifetime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 常规注入

            // 注册Scoped声明周期的类型
            services.AddScoped<ITestScoped, TestScoped>();
            // 注册Singleton声明周期的类型
            services.AddSingleton<ITestSignleton, TestSingleton>();
            // 注册Transient声明周期的类型
            services.AddTransient<ITestTransient, TestTransient>();

            #endregion

            #region 自注入(实例注入)

            //services.AddSingleton<OneSelfClass>();
            //services.AddSingleton(new OneSelfClass());
            services.AddSingleton(typeof(OneSelfClass));

            #endregion

            #region 工厂注入

            // 注册Scoped声明周期的类型
            services.AddScoped(provider => { return new TestScoped(); });
            // 注册Singleton声明周期的类型
            services.AddSingleton(provider => { return new TestSingleton(); });
            // 注册Transient声明周期的类型
            services.AddTransient(provider => { return new TestTransient(); });

            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
