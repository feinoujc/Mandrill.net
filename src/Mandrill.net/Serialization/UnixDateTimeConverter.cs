using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Serialization
{

    internal class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val;
            if (value is DateTime)
            {
                val = ((DateTime) value).ToUnixTime();
            }
            else
            {
                throw new JsonSerializationException("Expected date object value.");
            }
            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
                throw new JsonSerializationException("Wrong Token Type");

            var ticks = (long) reader.Value;
            return ticks.FromUnixTime();
        }
    }
}
