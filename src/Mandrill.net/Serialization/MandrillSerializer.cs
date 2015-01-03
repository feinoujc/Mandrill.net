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
            var guidConverter = new GuidConverter();
            settings.ContractResolver = new MandrillJsonContractResolver(unixTsConverter, guidConverter);
            settings.Converters.Add(unixTsConverter);
            settings.Converters.Add(guidConverter);
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonSerializer.Create(settings);
        }
    }
}