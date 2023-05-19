using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Utils
{
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
       /// 获取全局唯一业务流水号
       /// </summary>
       /// <returns></returns>
        public static string UniqueYwlsh()
        {
            var snowId = SqlSugar.SnowFlakeSingle.Instance.NextId();//获取雪花id
            return $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{snowId}";
        }
    }
}
