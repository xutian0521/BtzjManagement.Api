using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Filter
{
    public class LowerCaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            try
            {
                return name.ToLower();
            }
            catch
            {
                return name;
            }
        }
    }

    // 自定义一个派生自 DateTimeConverterBase 的转换器类
    public class FixedDateTimeConverter : DateTimeConverterBase
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss"; // 定义要转换的时间格式

        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //if (value is DateTime dateTime)
            //{
            //    writer.WriteValue(dateTime.ToUniversalTime().ToString(Format));
            //}
            throw new NotImplementedException();
        }
    }
}
