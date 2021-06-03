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
            #region ����ע��

            // ע��Scoped�������ڵ�����
            services.AddScoped<ITestScoped, TestScoped>();
            // ע��Singleton�������ڵ�����
            services.AddSingleton<ITestSignleton, TestSingleton>();
            // ע��Transient�������ڵ�����
            services.AddTransient<ITestTransient, TestTransient>();

            #endregion

            #region ��ע��(ʵ��ע��)

            //services.AddSingleton<OneSelfClass>();
            //services.AddSingleton(new OneSelfClass());
            services.AddSingleton(typeof(OneSelfClass));

            #endregion

            #region ����ע��

            // ע��Scoped�������ڵ�����
            services.AddScoped(provider => { return new TestScoped(); });
            // ע��Singleton�������ڵ�����
            services.AddSingleton(provider => { return new TestSingleton(); });
            // ע��Transient�������ڵ�����
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
