using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using BtzjManagement.Api.Utils;
using System;
using System.IO;
using System.Text;

namespace BtzjManagement.Api.Filter
{
    public static class MiddlewareExtension
    {
        public static void UseAESEncryption(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {

                // 保持原来的流
                var originalBody = context.Response.Body;
                var ms = new MemoryStream();
                context.Response.Body = ms;
                try
                {
                    bool decryptSuccess = false;
                    if (context.Request.Method.ToLower() == "get")
                    {

                        string requestQueryRaw = context.Request.QueryString.ToString();
                        if (string.IsNullOrEmpty(requestQueryRaw))
                        {
                            context.Items.Add("GetRequestIsEmpty", "1");
                        }
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in context.Request.Query)
                        {
                            string dec = AESHelper.DecryptByAES(item.Value.ToString().Replace(" ", "+"), out bool isDecryptionSuccessful);
                            decryptSuccess = isDecryptionSuccessful;

                            if (sb.Length > 0)
                            {
                                sb.Append("&");
                                sb.Append(item.Key);
                                sb.Append("=");
                                sb.Append(dec);
                            }
                            else
                            {
                                sb.Append("?");
                                sb.Append(item.Key);
                                sb.Append("=");
                                sb.Append(dec);
                            }
                        }
                        context.Request.QueryString = QueryString.FromUriComponent(sb.ToString());

                        if (decryptSuccess)
                        {
                            context.Items.Add("RequestQueryRaw", requestQueryRaw);
                            context.Items.Add("AESDecryptionSuccessful", "1");
                        }

                    }
                    else if (context.Request.Method.ToLower() == "post")
                    {
                        string requestBodyRaw = null;
                        //  这句很重要，开启读取 否者下面设置读取为0会失败
                        context.Request.EnableBuffering();
                        using (var reader = new System.IO.StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                        {
                            requestBodyRaw = await reader.ReadToEndAsync();
                        }
                        // 这里读取过body  Position是读取过几次  而此操作优于控制器先行 控制器只会读取Position为零次的
                        context.Request.Body.Position = 0;
                        if (string.IsNullOrEmpty(requestBodyRaw))
                        {
                            context.Items.Add("PostRequestIsEmpty", "1");
                        }
                        string dec = "";
                        dec = AESHelper.DecryptByAES(requestBodyRaw, out bool isDecryptionSuccessful);
                        decryptSuccess = isDecryptionSuccessful;
                        var RequestBody = new StreamReader(context.Request.BodyReader.AsStream()).ReadToEnd();//读取body

                        byte[] content1 = Encoding.UTF8.GetBytes(dec);
                        var requestBodyStream = new MemoryStream();//创建一个流 
                        requestBodyStream.Seek(0, SeekOrigin.Begin);//设置从0开始读取
                        requestBodyStream.Write(content1, 0, content1.Length);//把修改写入流中
                        context.Request.Body = requestBodyStream;//把修改后的内容赋值给请求body
                        context.Request.Body.Seek(0, SeekOrigin.Begin);
                        if (decryptSuccess)
                        {
                            context.Items.Add("RequestBodyRaw", requestBodyRaw);
                            context.Items.Add("AESDecryptionSuccessful", "1");
                        }

                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {

                }

                await next.Invoke();



                // 替换其中的内容
                ms.Seek(0, SeekOrigin.Begin);
                var reader2 = new StreamReader(ms);
                var str = reader2.ReadToEnd();
                //var doubleStr = str + str + "hello";
                StringBuilder Enc = new StringBuilder();
                //string Enc = "";
                if (!context.Items.TryGetValue("ResponseNotEncrypted", out object _vlaue))
                {
                    Enc.Append(context.Response.StatusCode == StatusCodes.Status200OK ? AESHelper.EncryptByAES(str) : str);

                    context.Response.Headers["X-Encryption-Response"] = "1";
                    context.Response.Headers["Access-Control-Expose-Headers"] = "X-Encryption-Response,dataSource";
                    context.Response.Headers["Access-Control-Allow-Headers"] = "X-Encryption-Request,dataSource,Content-Type";
                    context.Response.Headers["Content-Type"] = "text/plain; charset=utf-8";
                }
                else
                {
                    Enc.Append(str);
                }
                var buffer = Encoding.UTF8.GetBytes(Enc.ToString());

                var ms2 = new MemoryStream();
                ms2.Write(buffer, 0, buffer.Length);
                ms2.Seek(0, SeekOrigin.Begin);
                context.Response.Body.SetLength(buffer.Length);
                context.Response.Body.Position = 0;
                context.Response.ContentLength = buffer.Length;
                // 写入到原有的流中
                await ms2.CopyToAsync(originalBody);


            });
        }
    }
}
