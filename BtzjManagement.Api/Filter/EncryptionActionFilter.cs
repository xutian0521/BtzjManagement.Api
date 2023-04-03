using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BtzjManagement.Api.Filter
{
    public class EncryptionActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //需要加密
            if (context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(EncryptionAttribute)))
            {
                //中间件标记AES
                if (context.HttpContext.Items.TryGetValue("AES", out object _vlaue))
                {
                    return;
                }
            }
            else //不需要加密
            {
                //中间件标记AES
                if (context.HttpContext.Items.TryGetValue("AES", out object _vlaue))
                {
                    context.HttpContext.Response.StatusCode = 200;
                    context.Result = new ContentResult()
                    {
                        Content = "该接口，参数不需要加密"
                    };
                    return;
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //需要加密
            if (context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(EncryptionAttribute)))
            {
                //中间件没有解密成功且没有标记AES
                if (!context.HttpContext.Items.TryGetValue("AES", out object _vlaue))
                {
                    context.HttpContext.Items.Add("MarkNotEncrypted", "1");
                    context.HttpContext.Response.StatusCode = 200;
                    context.Result = new ContentResult()
                    {
                        Content = "非法请求,参数需要加密"
                    };
                    return;
                }
            }
            else //不需要加密
            {
                context.HttpContext.Items.Add("MarkNotEncrypted", "1");
            }
        }
    }
}
