using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillEventLocation
    {
        public string CountryShort { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string PostalCode { get; set; }

        public string Timezone { get; set; }

        [OnError]
        internal void OnError(StreamingContext context, ErrorContext errorContext)
        {
            errorContext.Handled = true;
        }
    }

}
