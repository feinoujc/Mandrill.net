using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillTrackingDomain
    {
        public string Domain { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastTestedAt { get; set; }

        public MandrillTrackingDomainCname Cname { get; set; }
        public bool ValidTracking { get; set; }
    }
}
