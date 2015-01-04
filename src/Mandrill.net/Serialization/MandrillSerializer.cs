using System;
using Newtonsoft.Json;

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
            var settings = new JsonSerializerSettings();

            var unixTsConverter = new UnixDateTimeConverter();
            settings.ContractResolver = new MandrillJsonContractResolver(unixTsConverter);
            settings.Converters.Add(unixTsConverter);
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return JsonSerializer.Create(settings);
        }
    }
}