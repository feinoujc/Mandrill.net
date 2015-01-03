using System;
using Newtonsoft.Json;

namespace Mandrill.Serialization
{
    internal class GuidConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string val;
            if (value is Guid)
            {
                val = ((Guid) value).ToString("N");
            }
            else
            {
                throw new JsonSerializationException("Expected guid object value.");
            }
            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new JsonSerializationException("Wrong Token Type");

            Guid result;
            if (!Guid.TryParseExact((string) reader.Value, "N", out result))
            {
                throw new JsonSerializationException("Invalid json guid value");
            }
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Guid) || objectType == typeof (Guid?);
        }
    }
}