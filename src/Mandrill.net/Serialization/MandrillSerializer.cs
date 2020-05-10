using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    public static class MandrillSerializer
    {
        public static JsonSerializer Instance { get; } = Create();

        private static JsonSerializer Create()
        {
            var settings = CreateSerializerSettings(NullValueHandling.Ignore);
            settings.Converters.Add(new MandrillMergeVarConverter(CreateSerializerSettings(NullValueHandling.Include)));
            return JsonSerializer.Create(settings);
        }

        private static JsonSerializerSettings CreateSerializerSettings(NullValueHandling nullValueHandling)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new MandrillJsonContractResolver() };
            settings.Converters.Add(new UnixDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter { NamingStrategy = new SnakeCaseNamingStrategy(), AllowIntegerValues = false });
            settings.NullValueHandling = nullValueHandling;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return settings;
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
