using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    internal sealed class UnixDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return Epoch.AddSeconds(reader.GetInt64());

            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.MinValue;

            return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value == DateTime.MinValue)
            {
                writer.WriteNumberValue(0);
                return;
            }

            writer.WriteNumberValue((long)(value.ToUniversalTime() - Epoch).TotalSeconds);
        }
    }
}
