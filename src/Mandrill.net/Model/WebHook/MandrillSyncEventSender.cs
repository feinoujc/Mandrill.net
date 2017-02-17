using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillSyncEventSender
    {
        public int? Sent { get; set; }

        public int? HardBounces { get; set; }

        public int? SoftBounces { get; set; }

        public int? Rejects { get; set; }

        public int? Complaints { get; set; }

        public int? Unsubs { get; set; }

        public int? Opens { get; set; }
        
        public int? Clicks { get; set; }

        public int? UniqueOpens { get; set; }

        public int? UniqueClicks { get; set; }

        public int? Reputation { get; set; }

        public string Address { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
