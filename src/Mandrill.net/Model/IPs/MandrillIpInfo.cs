using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillIpInfo
    {
        public string Ip { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public string Pool { get; set; }
        public string Domain { get; set; }
        public MandrillIpCustomDns CustomDns { get; set; }
        public MandrillIpWarmupStatus Warmup { get; set; }
    }
}
