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
    public static class EnumHelper
    {
        /// <summary>
        /// 获取类静态字段描述集合
        /// </summary>
        /// <typeparam name="ValueT">vlue值的类型</typeparam>
        /// <param name="enumType">类名</param>
        /// <returns>返回 key，value,描述</returns>
        public static List<(string key, ValueT value, string desc)> GetEnumDescriptionItemList<ValueT>(Type enumType)
        //where ValueT : class
        {
            FieldInfo[] enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<(string, ValueT, string)> result = new List<(string, ValueT, string)>();

            foreach (FieldInfo enumItem in enumItems)
            {
                //获取值
                var fieldValue = (ValueT)enumItem.GetValue(null);

                (string, ValueT, string) item = new(enumItem.Name, fieldValue, string.Empty);

                // 获取描述
                var attr = enumItem.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    item = (enumItem.Name, fieldValue, attr.Description);
                }
                result.Add(item);
            }
            return result;
        }


        /// <summary>
        /// 根据value值获取对应的key和描述
        /// </summary>
        /// <typeparam name="T">value的类型</typeparam>
        /// <param name="enumType">value所属类</param>
        /// <param name="value"></param>
        /// <param name="notFind">没有对应的value时显示的key和描述</param>
        /// <returns></returns>
        public static (string key, T value, string desc) GetEnumItemByValue<T>(Type enumType, object value, string notFind = "")
        {
            FieldInfo[] enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            object tem = 0;
            var key = notFind;
            var desc = notFind;
            if (value == null || string.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (typeof(T) == typeof(int) || typeof(T) == typeof(decimal) || typeof(T) == typeof(double))
                {
                    tem = 0;
                    return (key, (T)tem, desc);
                }
                tem = string.Empty;
                return (key, (T)tem, desc);
            }

            (string, T, string) result = new(string.Empty, (T)value, string.Empty);

            foreach (FieldInfo enumItem in enumItems)
            {
                //获取值
                var fieldValue = (object)enumItem.GetValue(null);

                if (new ValueOptT<T>(fieldValue) == new ValueOptT<T>(value))
                {
                    result = new(enumItem.Name, (T)fieldValue, string.Empty);

                    // 获取描述
                    var attr = enumItem.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;
                    if (attr != null && !string.IsNullOrEmpty(attr.Description))
                    {
                        result = (enumItem.Name, (T)fieldValue, attr.Description);
                    }
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据key获取对应的value值和描述
        /// </summary>
        /// <typeparam name="T">value值类型</typeparam>
        /// <param name="enumType">key所属类型</param>
        /// <param name="key"></param>
        /// <returns>没有对应的key返回异常</returns>
        public static (string key, T value, string desc) GetEnumItemByKey<T>(Type enumType, string key)
        {
            FieldInfo[] enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            object tem = 0;

            if (key == null || string.IsNullOrEmpty(key))
            {
                throw new Exception();
            }

            foreach (FieldInfo enumItem in enumItems)
            {
                if(enumItem.Name.ToUpper() == key.ToUpper())
                {
                    //获取值
                    var fieldValue = (object)enumItem.GetValue(null);
                    (string, T, string) result = new(enumItem.Name, (T)fieldValue, string.Empty);

                    // 获取描述
                    var attr = enumItem.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;
                    if (attr != null && !string.IsNullOrEmpty(attr.Description))
                    {
                        result = (enumItem.Name, (T)fieldValue, attr.Description);
                    }
                    return result;
                }
            }
            throw new Exception();
        }
    }


    public class ValueOptT<T>
    {
        private object _value { get; set; }
        public ValueOptT(object value)
        {
            _value = value;
        }

        public static bool operator ==(ValueOptT<T> a, ValueOptT<T> b)
        {
            if (typeof(T) == typeof(int))
            {
                return Convert.ToInt32(a._value) == Convert.ToInt32(b._value);
            }
            else if (typeof(T) == typeof(string))
            {
                return Convert.ToString(a._value) == Convert.ToString(b._value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return Convert.ToDecimal(a._value) == Convert.ToDecimal(b._value);
            }
            else if (typeof(T) == typeof(double))
            {
                return Convert.ToDouble(a._value) == Convert.ToDouble(b._value);
            }
            throw new Exception("ValueT‘==’ 重载异常");
        }

        public static bool operator !=(ValueOptT<T> a, ValueOptT<T> b)
        {
            if (typeof(T) == typeof(int))
            {
                return !(Convert.ToInt32(a._value) == Convert.ToInt32(b._value));
            }
            else if (typeof(T) == typeof(string))
            {
                return !(Convert.ToString(a._value) == Convert.ToString(b._value));
            }
            else if (typeof(T) == typeof(decimal))
            {
                return !(Convert.ToDecimal(a._value) == Convert.ToDecimal(b._value));
            }
            else if (typeof(T) == typeof(double))
            {
                return !(Convert.ToDouble(a._value) == Convert.ToDouble(b._value));
            }
            throw new Exception("ValueT‘!=’ 重载异常");
        }
    }

}