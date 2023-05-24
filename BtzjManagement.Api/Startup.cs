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
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Text.Json;
using System.ComponentModel;

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
            });
            services.AddControllers()       .AddJsonOptions(options =>
            {
                // 将所有字段名转换为小写
                options.JsonSerializerOptions.PropertyNamingPolicy = new LowercaseNamingPolicy();
                options.JsonSerializerOptions.Converters.Add(new MyDateTimeConverter());
            });

            services.AddCors(options => options.AddPolicy("AppCors", policy =>
            {
                var hosts = Configuration.GetValue<string>("AppHosts");
                policy.WithOrigins(hosts.Split(','))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            }));
            OracleConnector._connectionString = Configuration.GetConnectionString("conn");
            BaseDbContext.ConnectionString = Configuration.GetConnectionString("conn");
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                SybaseConnector.connectionString = Configuration.GetConnectionString("sybseConn");
            }
            catch { }
            services.AddSingleton<RuleService>();
            services.AddSingleton<SybaseService>();
            services.AddSingleton<DataTransferService>();
            services.AddSingleton<CorporationService>();
            services.AddSingleton<SysEnumService>();
            services.AddSingleton<BusiCorporationService>();
            services.AddSingleton<FlowProcService>();

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



