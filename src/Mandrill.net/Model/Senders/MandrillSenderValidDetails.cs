using Mandrill.Serialization;
using System.Text.Json.Serialization;
using System;

namespace Mandrill.Model
{
    public class MandrillSenderValidDetails
    {
        public bool Valid { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? ValidAfter { get; set; }

        public string Error { get; set; }
    }
}
