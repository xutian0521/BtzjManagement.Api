using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace BtzjManagement.Api.Utils
{
    /// <summary>
    /// json的操作类
    /// </summary>
    public class JsonHelper
    {
        #region 对象和json互转 

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isformatting">是否缩进JSON格式 </param>
        /// <returns>json字符串</returns>
        public static string ObjectToJson(object o, bool isformatting = false)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //忽略NULL值
                //DateFormatString = "yyyy-MM-dd HH:mm:ss"  //格式日期
            };
            string json = JsonConvert.SerializeObject(o, isformatting ? Formatting.Indented : Formatting.None, settings);
            //json = json.Replace("\"", "\'");
            json = json.Replace("\r", "").Replace("\n", "");
            return json;
        }
        /// <summary>
        /// 将对象集合序列化为JSON格式
        /// </summary>
        /// <param name="o">对象集合</param>
        /// <param name="isformatting">是否缩进JSON格式 </param>
        /// <returns>json字符串</returns>
        public static string ObjectToJson(List<object> o, bool isformatting = false)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //忽略NULL值
                //DateFormatString = "yyyy-MM-dd HH:mm:ss"  //格式日期
            };
            string json = JsonConvert.SerializeObject(o, isformatting ? Formatting.Indented : Formatting.None, settings);
            //json = json.Replace("\"", "\'");
            return json;
        }


        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// Student sdudent4 = JsonHelper.DeserializeJsonToObject<Student/>("{\"ID\":\"112\",\"Name\":\"石子儿\"}");
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T JsonToObject<T>(object json) where T : class
        {
            var strJson = json is string ? json.ToString() : ObjectToJson(json);
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(strJson);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// List<Student/> sdudentList4 = JsonHelper.DeserializeJsonToList<Student/>("[{\"ID\":\"112\",\"Name\":\"石子儿\"}]");
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> JsonToList<T>(object json) where T : class
        {
            string strJson;
            if (json is string)
            {
                strJson = json.ToString();
            }
            else
            {
                strJson = ObjectToJson(json);
            }
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(strJson);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }
        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// var tempEntity = new { ID = 0, Name = string.Empty }; 
        ///tempEntity = JsonHelper.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempEntity);
        ///var tempStudent = new Student();
        ///tempStudent = JsonHelper.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempStudent);
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T JsonToAnonymous<T>(dynamic json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
        #endregion


        #region DataTable转实体 
        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="row">DataRow</param>
        /// <returns>实体</returns> 
        public static T DataRowToEntity<T>(DataRow row) where T : new()
        {
            T entity = new T();
            foreach (var item in entity.GetType().GetProperties())
            {
                if (row.Table.Columns.Contains(item.Name))
                {   //取值   
                    object value = row[item.Name];
                    //如果非空，则赋给对象的属性   
                    if (value != DBNull.Value)
                    {
                        var valueStr = value.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(valueStr))
                        {
                            item.SetValue(entity, GetParseValue(item.PropertyType, valueStr), null);
                        }
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>实体</returns>
        public static List<T> DataTableToEntities<T>(DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        //取值   
                        object value = row[item.Name];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                            item.SetValue(entity, value, null);
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>实体</returns>
        public static List<T> DataTableToEntitiesMatch<T>(DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        //取值   
                        object value = row[item.Name];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(value.ToString()))
                            {
                                Type type = item.PropertyType;
                                //判断type类型是否为泛型，因为nullable是泛型类,
                                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                                {
                                    type = Nullable.GetUnderlyingType(type);
                                }
                                item.SetValue(entity, Convert.ChangeType(value, type), null);
                                //item.SetValue(entity, value, null);
                            }
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object GetParseValue(Type type, string value)
        {
            switch (type.ToString().ToUpper())
            {
                case DataTypeString.Decimal:
                    return Convert.ToDecimal(value);
                case DataTypeString.NullableDecimal:
                    return Convert.ToDecimal(value);
                case DataTypeString.DataTime:
                case DataTypeString.NullableDateTime:
                    return Convert.ToDateTime(value);
                case DataTypeString.Int:
                case DataTypeString.NullableInt:
                    return Convert.ToInt32(value);
                case DataTypeString.String:
                    return value;
                case DataTypeString.Single:
                    return Convert.ToSingle(value);
                case DataTypeString.Boolean:
                case DataTypeString.NullableBoolean:
                    return Convert.ToBoolean(value);
                case DataTypeString.Double:
                    return Convert.ToDouble(value);
                case DataTypeString.Guid:
                    return new Guid(value);
                case DataTypeString.NullableGuid:
                    return new Guid(value);
                case DataTypeString.Byte:
                case DataTypeString.NullByte:
                    return Convert.ToByte(value);
                default:
                    return value;
            }
        }
         
        #endregion

    }

    /// <summary>
    /// 数据类型字符串
    /// </summary>
    public struct DataTypeString
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DataTime = "SYSTEM.DATETIME";
        /// <summary>
        /// 
        /// </summary>
        public const string NullableDateTime = "SYSTEM.NULLABLE`1[SYSTEM.DATETIME]";
        /// <summary>
        /// 
        /// </summary>
        public const string NullableInt = "SYSTEM.NULLABLE`1[SYSTEM.INT32]";
        /// <summary>
        /// 
        /// </summary>
        public const string NullableDecimal = "SYSTEM.NULLABLE`1[SYSTEM.DECIMAL]";
        /// <summary>
        /// 
        /// </summary>
        public const string Int = "SYSTEM.INT32";
        /// <summary>
        /// 
        /// </summary>
        public const string Int64 = "SYSTEM.INT64";
        /// <summary>
        /// 
        /// </summary>
        public const string String = "SYSTEM.STRING";
        /// <summary>
        /// 
        /// </summary>
        public const string Decimal = "SYSTEM.DECIMAL";
        /// <summary>
        /// 
        /// </summary>
        public const string Single = "SYSTEM.SINGLE";
        /// <summary>
        /// 
        /// </summary>
        public const string Boolean = "SYSTEM.BOOLEAN";
        /// <summary>
        /// 
        /// </summary>
        public const string NullableBoolean = "SYSTEM.NULLABLE`1[SYSTEM.BOOLEAN]";
        /// <summary>
        /// 
        /// </summary>
        public const string Double = "SYSTEM.DOUBLE";
        /// <summary>
        /// 
        /// </summary>
        public const string Guid = "SYSTEM.GUID";
        /// <summary>
        /// 
        /// </summary>
        public const string NullableGuid = "SYSTEM.NULLABLE`1[SYSTEM.GUID]";
        /// <summary>
        /// 
        /// </summary>
        public const string EnumerableString = "SYSTEM.COLLECTIONS.GENERIC.IENUMERABLE`1[SYSTEM.STRING]";
        /// <summary>
        /// 
        /// </summary>
        public const string Byte = "SYSTEM.BYTE";
        /// <summary>
        /// 
        /// </summary>
        public const string NullByte = "SYSTEM.NULLABLE`1[SYSTEM.BYTE]";
    }

}

