using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace BtzjManagement.Api.Filter
{
    /// <summary>
    /// 时间类型json转换器类型
    /// </summary>
    public class DatetimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 重写读取方法
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="typeToConvert">typeToConvert</param>
        /// <param name="options">options</param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    return date;
            }
            return reader.GetDateTime();
        }
        /// <summary>
        /// 重写写入方法
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value">value</param>
        /// <param name="options">options</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm"));
        }
    }
}
