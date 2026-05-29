using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillEventLocation
    {
        public string CountryShort { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        [JsonConverter(typeof(LenientDoubleConverter))]
        public double? Latitude { get; set; }
        [JsonConverter(typeof(LenientDoubleConverter))]
        public double? Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Timezone { get; set; }
    }
}
