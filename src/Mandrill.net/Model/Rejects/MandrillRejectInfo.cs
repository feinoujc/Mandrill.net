using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillRejectInfo
    {
        public string Email { get; set; }

        public string Reason { get; set; }

        public string Detail { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastEventAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? ExpiresAt { get; set; }

        public bool Expired { get; set; }

        public MandrillSenderDemographics Sender { get; set; }

        public string Subaccount { get; set; }
    }
}
