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
using NPinyin;
using BtzjManagement.Api.Enum;

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
        /// 账号左边补零
        /// </summary>
        /// <param name="num">账号</param>
        /// <param name="lenth">填充后达到几位数长度的字符串</param>
        /// <returns></returns>
        public static string PaddingLeftZero(int num,int lenth)
        {
            //int billNo = 10;
            string billNo_ = num.ToString();
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
                var num18 = PaddingLeftZero(Convert.ToInt32($"{dwzh}{grzh}"), 18);
                return $"A{num18}{numLast}";
            }

            return $"A{zjhm.ToUpper()}{numLast}";
        }

        /// <summary>
        /// 个人账号生成
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="grzhInt"></param>
        /// <returns></returns>
        public static string PersonGrzhGenerate(string dwzh,int grzhInt)
        {
            var grzhNum = PaddingLeftZero(grzhInt, 5);
            return dwzh + grzhNum;
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

        /// <summary>
        /// 将汉字字符串转为拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertChineseToPinYin(string str)
        {
            string temp = "";
            if (string.IsNullOrEmpty(str))
                return temp;
            foreach (var item in str.ToCharArray())
            {
                var a = Pinyin.GetPinyin(item);
                temp += a;
            }
            return temp;
        }

        /// <summary>
        /// 将汉字字符串转为拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsDw">是单位名称</param>
        /// <returns></returns>
        public static string ConvertChineseToPinYinShouZiMu(string str,bool IsDw = false)
        {
            string temp = "";
            if (string.IsNullOrEmpty(str))
                return temp;
            if (IsDw)
            {
                str = str.Replace("行", "h");
            }
            
            foreach (var item in str.ToCharArray())
            {
                var a = Pinyin.GetPinyin(item).ToArray().FirstOrDefault().ToString();
                temp += a;
            }
            return temp;
        }

        #region sybase对应类型转贯标
        /// <summary>
        /// 单位账号状态
        /// </summary>
        public static string DwzhztConstSwitch(string org_dwzhzt)
        {
            switch (org_dwzhzt)
            {
                case DwzhztConstOld.正常:
                    return DwzhztConst.正常;
                case DwzhztConstOld.封存:
                    return DwzhztConst.封存;
                case DwzhztConstOld.作废:
                    return DwzhztConst.其他;
                default:
                    return org_dwzhzt;
            }
        }

        /// <summary>
        /// 个人账号状态
        /// </summary>
        /// <param name="org_grzhzt"></param>
        /// <returns></returns>
        public static string GrzhztConstSwitch(string org_grzhzt)
        {
            switch (org_grzhzt)
            {
                case GrzhztConstOld.正常:
                    return GrzhztConst.正常;
                case GrzhztConstOld.封存:
                    return GrzhztConst.封存;
                case GrzhztConstOld.丧失劳动能力并与单位终止关系:
                    return GrzhztConst.提取销户;
                case GrzhztConstOld.户口迁出本地或出境定居:
                    return GrzhztConst.提取销户;
                case GrzhztConstOld.离休退休:
                    return GrzhztConst.提取销户;
                case GrzhztConstOld.转出:
                    return GrzhztConst.外部转出销户;
                case GrzhztConstOld.其他:
                    return GrzhztConst.其他;
                case GrzhztConstOld.其他原因销户:
                    return GrzhztConst.其他;
                default:
                    return org_grzhzt;
            }
        }

        /// <summary>
        /// 个人销户原因对照
        /// </summary>
        /// <param name="org_grzhzt"></param>
        /// <returns></returns>
        public static string GrXiaoHuReasonConstSwitch(string org_grzhzt)
        {
            switch (org_grzhzt)
            {
                case GrzhztConstOld.正常:
                    return "";
                case GrzhztConstOld.封存:
                    return "";
                case GrzhztConstOld.丧失劳动能力并与单位终止关系:
                    return DrawReasonConst.完全丧失劳动能力_并与单位终止劳动关系;
                case GrzhztConstOld.户口迁出本地或出境定居:
                    return DrawReasonConst.户口迁出所在市_县或出境定居;
                case GrzhztConstOld.离休退休:
                    return DrawReasonConst.离休_退休;
                case GrzhztConstOld.转出:
                    return "";
                case GrzhztConstOld.其他:
                    return DrawReasonConst.其他;
                case GrzhztConstOld.其他原因销户:
                    return DrawReasonConst.其他;
                default:
                    return org_grzhzt;
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        /// <param name="org_dwzhzt"></param>
        /// <returns></returns>
        public static string XingBieConstSwitch(string org_dwzhzt)
        {
            switch (org_dwzhzt)
            {
                case XingBieConstOld.女:
                    return XingBieConst.女性;
                case XingBieConstOld.男:
                    return XingBieConst.男性;
                default:
                    return org_dwzhzt;
            }
        }
        #endregion

        /// <summary>
        /// 获取月缴存额
        /// </summary>
        /// <param name="jcbl">缴存比例，不是百分比</param>
        /// <param name="grjcsj">个人缴存基数</param>
        /// <param name="method">计算方法-对应 CalcMethodConst类型</param>
        /// <returns></returns>
        public static decimal GetYjce(decimal jcbl, decimal grjcsj, string method)
        {
            var jcbl_Percent = jcbl / 100;
            switch (method)
            {
                case CalcMethodConst.舍入到分:
                    return Math.Truncate(grjcsj * jcbl_Percent * 100) / 100;
                case CalcMethodConst.舍入到角:
                    return Math.Truncate(grjcsj * jcbl_Percent * 10) / 10;
                case CalcMethodConst.舍入到元:
                    return Math.Truncate(grjcsj * jcbl_Percent);

                case CalcMethodConst.四舍五入到分:
                    return Math.Round(grjcsj * jcbl_Percent, 2);
                case CalcMethodConst.四舍五入到角:
                    return Math.Round(grjcsj * jcbl_Percent, 1);
                case CalcMethodConst.四舍五入到元:
                    return Math.Round(grjcsj * jcbl_Percent);

                case CalcMethodConst.见厘进分:
                    return Math.Ceiling(grjcsj * jcbl_Percent * 100) / 100;
                case CalcMethodConst.见分进角:
                    return Math.Ceiling(grjcsj * jcbl_Percent * 10) / 10;
                case CalcMethodConst.见角进元:
                    return Math.Ceiling(grjcsj * jcbl_Percent);
                default:
                    throw new Exception();
            }


            //var jcbl = 15.233M;
            //var grjcjs = 5555.23M;
            //var www = jcbl / 100 * grjcjs;
            //var tt = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.舍入到分);
            //var tt11 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.舍入到角);
            //var tt22 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.舍入到元);
            //var tt33 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.四舍五入到分);
            //var tt44 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.四舍五入到角);
            //var tt55 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.四舍五入到元);
            //var tt66 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.见厘进分);
            //var tt77 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.见分进角);
            //var tt88 = Utils.Common.GetYjce(jcbl, grjcjs, Enum.CalcMethodConst.见角进元);


        }
    }
}
