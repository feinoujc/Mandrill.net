using System;
using System.Collections.Generic;
using Mandrill.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mandrill.Serialization
{
    internal class MandrillMergeVarContentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var content = (MandrillMergeVarContent) value;
            if (content != null)
            {
                if (content.ValueAsArray != null)
                {
                    serializer.Serialize(writer, content.ValueAsArray);
                }
                else if (content.ValueAsString != null)
                {
                    serializer.Serialize(writer, content.ValueAsString);
                }
                else
                {
                    writer.WriteNull();
                }
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.String)
            {
                return new MandrillMergeVarContent(JToken.Load(reader).Value<string>());
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                var jArray = JArray.Load(reader);
                return new MandrillMergeVarContent(jArray.ToObject<List<Dictionary<string, object>>>(serializer));
            }

            throw new JsonSerializationException("Unexpected tokenType " + reader.TokenType);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (MandrillMergeVarContent).IsAssignableFrom(objectType);
        }
    }
}