using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class MandrillHeaderValueConverter : JsonConverter<MandrillHeaderValue>
    {
        public override MandrillHeaderValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return default;

                case JsonTokenType.String:
                    return new MandrillHeaderValue(reader.GetString());

                case JsonTokenType.StartArray:
                    var values = new List<string>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        values.Add(reader.TokenType == JsonTokenType.Null ? null : reader.GetString());
                    }
                    return new MandrillHeaderValue(values);

                default:
                    throw new JsonException($"Unexpected token {reader.TokenType} when reading MandrillHeaderValue");
            }
        }

        public override void Write(Utf8JsonWriter writer, MandrillHeaderValue value, JsonSerializerOptions options)
        {
            var values = value.Values;
            if (values.Count == 1)
            {
                writer.WriteStringValue(values[0]);
            }
            else
            {
                writer.WriteStartArray();
                foreach (var v in values)
                {
                    if (v == null)
                        writer.WriteNullValue();
                    else
                        writer.WriteStringValue(v);
                }
                writer.WriteEndArray();
            }
        }
    }
}
