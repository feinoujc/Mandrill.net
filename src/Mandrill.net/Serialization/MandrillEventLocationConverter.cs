using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mandrill.Model;

namespace Mandrill.Serialization
{
    internal class MandrillEventLocationConverter : JsonConverter<MandrillEventLocation>
    {
        public override MandrillEventLocation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new MandrillEventLocation();
            if (reader.TokenType == JsonTokenType.Null)
            {
                return result;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "country_short":
                        result.CountryShort = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "country":
                        result.Country = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "region":
                        result.Region = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "city":
                        result.City = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "latitude":
                        if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out var latitude))
                        {
                            result.Latitude = latitude;
                        }
                        break;
                    case "longitude":
                        if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out var longitude))
                        {
                            result.Longitude = longitude;
                        }
                        break;
                    case "postal_code":
                        result.PostalCode = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case "timezone":
                        result.Timezone = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, MandrillEventLocation value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteNullableString(writer, "country_short", value.CountryShort, options);
            WriteNullableString(writer, "country", value.Country, options);
            WriteNullableString(writer, "region", value.Region, options);
            WriteNullableString(writer, "city", value.City, options);
            if (value.Latitude.HasValue)
            {
                writer.WriteNumber("latitude", value.Latitude.Value);
            }
            if (value.Longitude.HasValue)
            {
                writer.WriteNumber("longitude", value.Longitude.Value);
            }
            WriteNullableString(writer, "postal_code", value.PostalCode, options);
            WriteNullableString(writer, "timezone", value.Timezone, options);
            writer.WriteEndObject();
        }

        private static void WriteNullableString(Utf8JsonWriter writer, string propertyName, string value, JsonSerializerOptions options)
        {
            var ignoreNulls = options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull ||
                              options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingDefault;
            if (value == null && ignoreNulls)
            {
                return;
            }

            if (value == null)
            {
                writer.WriteNull(propertyName);
            }
            else
            {
                writer.WriteString(propertyName, value);
            }
        }
    }
}
