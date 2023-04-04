using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
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
    }
}
