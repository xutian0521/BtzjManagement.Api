using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BtzjManagement.Api.Utils
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 将cp850字符串转成gbk
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Cp850ToGBK(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Encoding.GetEncoding("gbk").GetString(Encoding.GetEncoding("cp850").GetBytes(str));
            }
            return str;
        }


        /// <summary>
        /// 将GBK字符串转为cp850
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GBKToCp850(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Encoding.GetEncoding("cp850").GetString(Encoding.GetEncoding("gbk").GetBytes(str));
            }
            return str;
        }

        /// <summary>
        /// 单位账号左边补零
        /// </summary>
        /// <param name="dwzh">单位账号</param>
        /// <param name="lenth">填充后达到几位数长度的字符串</param>
        /// <returns></returns>
        public static string PaddingDwzh(int dwzh,int lenth)
        {
            //int billNo = 10;
            string billNo_ = dwzh.ToString();
            //指定billNo为6位，不足位数时左边补零
            billNo_ = billNo_.PadLeft(lenth, '0');
            return billNo_;
        }

        /// <summary>
        /// 个人custid规则 A+18位数+0
        /// </summary>
        /// <param name="zjhm"></param>
        /// <param name="dwzh"></param>
        /// <param name="grzh"></param>
        /// <param name="numLast"></param>
        /// <param name="isOldData"></param>
        /// <returns></returns>
        public static string PersonCustIDGenerate(string zjhm, string dwzh = "", string grzh = "", bool isOldData = false, int numLast = 0)
        {
            if (isOldData)
            {
                var num18 = PaddingDwzh(Convert.ToInt32($"{dwzh}{grzh}"), 18);
                return $"A{num18}{numLast}";
            }

            return $"A{zjhm.ToLower()}{numLast}";
        }

       /// <summary>
       /// 获取全局唯一业务流水号
       /// </summary>
       /// <returns></returns>
        public static string UniqueYwlsh()
        {
            var snowId = SqlSugar.SnowFlakeSingle.Instance.NextId();//获取雪花id
            return $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{snowId}";
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="rawPass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string MD5Encoding(string rawPass, string salt)
        {
            if (salt == null) return rawPass;
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass + "{" + salt + "}");
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomNumber()
        {
            Random ran = new Random();
            int number = ran.Next(0, 9999);
            string num;
            if (number < 1000)
            {
                num = number.ToString("0000");
            }
            else
            {
                num = number.ToString();
            }
            return num;
        }

        /// <summary>
        /// 根据文本生成图形验证码
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 25.0), 32);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 18, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetIP(HttpContext httpContext)
        {
            string XForwardedFor = httpContext.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(XForwardedFor))
            {
                string[] ips = XForwardedFor.Trim().Split(',');
                if (ips.Length > 0 && ips[0].Length > 7)
                    return ips[0].Split(' ')[0];
            }

            string ip = httpContext.Request.Headers["RemoteIp"];
            if (ip == null)
            {
                ip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip.Replace("'", "");
        }
    }
}
