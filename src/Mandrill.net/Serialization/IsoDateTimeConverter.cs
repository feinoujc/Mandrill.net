using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class IsoDateTimeConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => typeToConvert == typeof(DateTime) ? new IsoDateTimeInner() : new NullableIsoDateTimeInner();

        private sealed class IsoDateTimeInner : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return DateTime.MinValue;
                }

                return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToUniversalTime().ToString("O"));
        }

        private sealed class NullableIsoDateTimeInner : JsonConverter<DateTime?>
        {
            private readonly IsoDateTimeInner _inner = new();

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
