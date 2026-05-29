using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillIpWarmupStatus
    {
        public bool WarmingUp { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? StartAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? EndAt { get; set; }
    }
}
