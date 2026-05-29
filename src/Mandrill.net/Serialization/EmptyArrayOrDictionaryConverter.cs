using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class EmptyArrayOrDictionaryConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(Dictionary<,>) || genericType == typeof(IDictionary<,>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var args = typeToConvert.GetGenericArguments();
            var converterType = typeof(EmptyArrayOrDictionaryConverterInner<,>).MakeGenericType(args[0], args[1]);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }

        private sealed class EmptyArrayOrDictionaryConverterInner<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
        {
            public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Skip();
                    return CreateDictionary();
                }

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                var dictionary = CreateDictionary();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return dictionary;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    var propertyName = reader.GetString()!;
                    var key = ReadKey(propertyName);
                    reader.Read();
                    dictionary[key] = ReadValue(ref reader, options);
                }

                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (var kvp in value)
                {
                    writer.WritePropertyName(kvp.Key.ToString()!);
                    JsonSerializer.Serialize(writer, kvp.Value, options);
                }
                writer.WriteEndObject();
            }

            private static Dictionary<TKey, TValue> CreateDictionary()
            {
                if (typeof(TKey) == typeof(string))
                {
                    return new Dictionary<TKey, TValue>((IEqualityComparer<TKey>)StringComparer.OrdinalIgnoreCase);
                }

                return new Dictionary<TKey, TValue>();
            }

            private static TKey ReadKey(string propertyName)
            {
                if (typeof(TKey) == typeof(string))
                {
                    return (TKey)(object)propertyName;
                }

                return (TKey)Convert.ChangeType(propertyName, typeof(TKey), CultureInfo.InvariantCulture);
            }

            private static TValue ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                if (typeof(TValue) == typeof(string))
                {
                    return (TValue)(object)ReadStringValue(ref reader);
                }

                if (typeof(TValue) == typeof(object))
                {
                    return (TValue)ReadObjectValue(ref reader);
                }

                return JsonSerializer.Deserialize<TValue>(ref reader, options)!;
            }

            private static string ReadStringValue(ref Utf8JsonReader reader)
            {
                return reader.TokenType switch
                {
                    JsonTokenType.Null => null,
                    JsonTokenType.String => reader.GetString(),
                    JsonTokenType.Number or JsonTokenType.True or JsonTokenType.False => ReadRawText(ref reader),
                    JsonTokenType.StartObject or JsonTokenType.StartArray => ReadRawText(ref reader),
                    _ => throw new JsonException(),
                };
            }

            private static object ReadObjectValue(ref Utf8JsonReader reader)
            {
                return reader.TokenType switch
                {
                    JsonTokenType.Null => null,
                    JsonTokenType.String => reader.GetString(),
                    JsonTokenType.True => true,
                    JsonTokenType.False => false,
                    JsonTokenType.Number => reader.TryGetInt64(out var longValue)
                        ? longValue
                        : reader.TryGetDecimal(out var decimalValue)
                            ? decimalValue
                            : reader.GetDouble(),
                    JsonTokenType.StartObject or JsonTokenType.StartArray => ReadElement(ref reader),
                    _ => throw new JsonException(),
                };
            }

            private static string ReadRawText(ref Utf8JsonReader reader)
            {
                using var document = JsonDocument.ParseValue(ref reader);
                return document.RootElement.ToString();
            }

            private static JsonElement ReadElement(ref Utf8JsonReader reader)
            {
                using var document = JsonDocument.ParseValue(ref reader);
                return document.RootElement.Clone();
            }
        }
    }
}
