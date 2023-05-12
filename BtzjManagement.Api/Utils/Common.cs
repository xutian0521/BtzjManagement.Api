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
    }
}
