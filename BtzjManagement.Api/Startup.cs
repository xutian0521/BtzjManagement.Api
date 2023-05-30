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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //修改属性名称的序列化方式，字段全小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new LowerCaseNamingStrategy()
                };

                //修改时间的序列化方式
                options.SerializerSettings.Converters.Add(new FixedDateTimeConverter());
                //忽略循环引用
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //我们将 NullValueHandling 设置为 Ignore，表示在序列化时忽略 null 值。如果您希望将 null 值序列化为 JSON 字符串 "null"，则可以将其设置为 Include。
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });
            services.AddSwaggerDocument();
            services.AddCors(options => options.AddPolicy("AppCors", policy =>
            {
                var hosts = Configuration.GetValue<string>("AppHosts");
                policy.WithOrigins(hosts.Split(','))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            }));
            //OracleConnector._connectionString = Configuration.GetConnectionString("conn");
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

            services.AddSingleton<BusiCorporationService>();
            services.AddSingleton<FlowProcService>();
            services.AddSingleton<AccountService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AppCors");

            app.UseRouting();
            app.UseOpenApi();
            app.UseSwaggerUi3();

                        app.UseAESEncryption();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}



