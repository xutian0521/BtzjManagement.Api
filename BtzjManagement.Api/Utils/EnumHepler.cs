using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Utils
{
    /// <summary>
    /// 枚举操作类
    /// </summary>
    public static class EnumHepler
    {
        /// <summary>
        /// 获取类静态字段描述集合
        /// </summary>
        /// <typeparam name="ValueT">vlue值的类型</typeparam>
        /// <param name="enumType">类名</param>
        /// <returns>返回 key，value,描述</returns>
        public static List<Tuple<string, ValueT, string>> GetEnumDescriptionItems<ValueT>(Type enumType) 
            where ValueT :class
        {
            FieldInfo[] enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Tuple<string, ValueT, string>> result = new List<Tuple<string, ValueT, string>>();

            foreach (FieldInfo enumItem in enumItems)
            {
                //获取值
                var fieldValue = (ValueT)enumItem.GetValue(null);

                Tuple<string, ValueT, string> item = new Tuple<string, ValueT, string>(enumItem.Name,fieldValue,string.Empty);

                // 获取描述
                var attr = enumItem.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    item = new Tuple<string, ValueT, string>(enumItem.Name, fieldValue, attr.Description);
                }
                result.Add(item);
            }
            return result;
        }
    }
}