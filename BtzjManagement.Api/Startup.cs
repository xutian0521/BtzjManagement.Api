using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BtzjManagement.Api.Filter;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BtzjManagement.Api.Services;

namespace BtzjManagement.Api
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

            services.AddMvc(option =>
            {
                option.Filters.Add<UserAuthorizeFilter>();
                option.Filters.Add<EncryptionActionFilter>();
            })
            .AddJsonOptions(options =>
                //格式化日期时间格式
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter()));
            ;
            services.AddControllers();

            services.AddCors(options => options.AddPolicy("AppCors", policy =>
            {
                var hosts = Configuration.GetValue<string>("AppHosts");
                policy.WithOrigins(hosts.Split(','))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            }));
            OracleConnector._connectionString = Configuration.GetConnectionString("conn");
            services.AddSingleton<RuleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AppCors");
            app.UseAESEncryption();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
