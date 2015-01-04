using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Serialization
{
    public static class MandrillSerializer
    {
        private static readonly Lazy<JsonSerializer> LazyJsonSerializer = new Lazy<JsonSerializer>(CreateSerializer);

        public static JsonSerializer Instance
        {
            get { return LazyJsonSerializer.Value; }
        }

        private static JsonSerializer CreateSerializer()
        {
            var settings = new JsonSerializerSettings {ContractResolver = new MandrillJsonContractResolver()};

            settings.Converters.Add(new UnixDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter {CamelCaseText = true, AllowIntegerValues = false});
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return JsonSerializer.Create(settings);
        }
    }
}