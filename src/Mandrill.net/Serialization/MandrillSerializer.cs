using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    public static class MandrillSerializer
    {
        [Obsolete("Use CreateDefault or Create factory instead")]
        public static JsonSerializer Instance { get; } = CreateDefault();

        public static JsonSerializer CreateDefault() => Create(CreateSerializerSettings(NullValueHandling.Include));

        public static JsonSerializer Create(JsonSerializerSettings contentSerializerSettings)
        {
            var settings = CreateSerializerSettings(NullValueHandling.Ignore);
            settings.Converters.Add(new MandrillMergeVarConverter(contentSerializerSettings));
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

        internal static JsonSerializerSettings CreateDefaultContentSerializerSettings() => CreateSerializerSettings(NullValueHandling.Include);
    }

    public static class MandrillSerializer<T>
    {
        private static readonly JsonSerializer _serializer = MandrillSerializer.CreateDefault();

        public static T Deserialize(JsonReader reader)
        {
            return _serializer.Deserialize<T>(reader);
        }

        public static void Serialize(JsonWriter writer, T value)
        {
            _serializer.Serialize(writer, value);
        }
    }
}
