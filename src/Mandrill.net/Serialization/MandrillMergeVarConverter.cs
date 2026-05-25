using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mandrill.Model;

namespace Mandrill.Serialization
{
    internal class MandrillMergeVarConverter : JsonConverter<MandrillMergeVar>
    {
        private readonly JsonSerializerOptions _contentOptions;

        public MandrillMergeVarConverter(JsonSerializerOptions contentOptions)
        {
            _contentOptions = contentOptions;
        }

        public override MandrillMergeVar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var mergeVar = new MandrillMergeVar();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return mergeVar;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "name":
                        mergeVar.Name = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "content":
                        mergeVar.Content = reader.TokenType switch
                        {
                            JsonTokenType.Null => null,
                            JsonTokenType.String => reader.GetString(),
                            _ => JsonSerializer.Deserialize<object>(ref reader, options),
                        };
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, MandrillMergeVar value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("name", value.Name);
            writer.WritePropertyName("content");
            if (value.Content == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Content, _contentOptions);
            }
            writer.WriteEndObject();
        }
    }
}
