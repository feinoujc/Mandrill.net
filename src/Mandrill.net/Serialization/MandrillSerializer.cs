using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    public static class MandrillSerializer
    {
        private static readonly Lazy<JsonSerializer> LazyJsonSerializer = new Lazy<JsonSerializer>(CreateSerializer);

        public static JsonSerializer Instance => LazyJsonSerializer.Value;

        private static JsonSerializer CreateSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new MandrillJsonContractResolver()
            };

            settings.Converters.Add(new UnixDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter { NamingStrategy = new SnakeCaseNamingStrategy(), AllowIntegerValues = false });
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
   
            return JsonSerializer.Create(settings);
        }
    }

    public static class MandrillSerializer<T>
    {
        public static T Deserialize(JsonReader reader)
        {
            return MandrillSerializer.Instance.Deserialize<T>(reader);
        }

        public static void Serialize(JsonWriter writer, T value)
        {
            MandrillSerializer.Instance.Serialize(writer, value);
        }
    }
}
