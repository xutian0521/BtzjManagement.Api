using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : BaseController
    {
        RuleService _ruleService;
        /// <summary>
        /// ctor
        /// </summary>
        public SystemController(IConfiguration configuration, RuleService ruleService) : base(configuration)
        {
            _ruleService = ruleService;
        }

        [HttpPost("events")]
        public async Task GetEvents()
        {
            // 从请求体中解析出 JSON 对象
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();
                // 将 JSON 对象转换为 Event Stream 数据
                var eventStreamData = JsonSerializer.Serialize(json);
                // 设置 HTTP 响应头
                Response.Headers.Add("Content-Type", "text/event-stream");
                Response.Headers.Add("Cache-Control", "no-cache");
                // 将 Event Stream 数据写入 HTTP 响应体
                string k = "HVsnxfhTtZiQWDvZ", v = "HutdrmyYyQpBDcWK";
                string key = AESHelper.EncryptByAESBy(AESHelper.key, k, v);
                string iv = AESHelper.EncryptByAESBy(AESHelper.iv, k, v);
                var data = key + "," + iv;
                for (int i = 0; i < data.Length; i++)
                {
                    await Response.WriteAsync(data[i].ToString());
                    await Response.Body.FlushAsync();
                }
                //await Response.WriteAsync(eventStreamData);
            }
        }

        /// <summary>
        /// 获取账务基本信息配置
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("GetSysConfig")]
        public v_ApiResult GetSysConfig()
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var rr = _ruleService.GetSysconfig(CityCent());
                result.Code = rr.code;
                result.Message = rr.message;
                result.Content = rr.model;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
