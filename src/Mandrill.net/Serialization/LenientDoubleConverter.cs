using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    /// <summary>
    /// Deserializes a nullable double, returning null for any non-numeric token.
    /// Mandrill webhooks occasionally send non-numeric values (e.g., IP addresses)
    /// in fields documented as floats (latitude/longitude).
    /// </summary>
    internal class LenientDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out var value))
            {
                return value;
            }

            // Skip unexpected tokens (strings, booleans, arrays, objects, etc.)
            reader.Skip();
            return null;
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
