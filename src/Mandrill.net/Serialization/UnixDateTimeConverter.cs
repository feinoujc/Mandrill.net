using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class UnixDateTimeConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => typeToConvert == typeof(DateTime) ? new UnixDateTimeInner() : new NullableUnixDateTimeInner();

        private sealed class UnixDateTimeInner : JsonConverter<DateTime>
        {
            private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    return Epoch.AddSeconds(reader.GetInt64());
                }

                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return DateTime.MinValue;
                }

                return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                if (value == DateTime.MinValue)
                {
                    writer.WriteNumberValue(0);
                    return;
                }

                var delta = value.ToUniversalTime() - Epoch;
                writer.WriteNumberValue((long)delta.TotalSeconds);
            }
        }

        private sealed class NullableUnixDateTimeInner : JsonConverter<DateTime?>
        {
            private readonly UnixDateTimeInner _inner = new();

            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => reader.TokenType == JsonTokenType.Null ? null : _inner.Read(ref reader, typeof(DateTime), options);

            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }

                _inner.Write(writer, value.Value, options);
            }
        }
    }
}
