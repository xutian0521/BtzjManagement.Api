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
                //�޸��������Ƶ����л���ʽ���ֶ�ȫСд
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new LowerCaseNamingStrategy()
                };

                //�޸�ʱ������л���ʽ
                options.SerializerSettings.Converters.Add(new FixedDateTimeConverter());
                //����ѭ������
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //���ǽ� NullValueHandling ����Ϊ Ignore����ʾ�����л�ʱ���� null ֵ�������ϣ���� null ֵ���л�Ϊ JSON �ַ��� "null"������Խ�������Ϊ Include��
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



