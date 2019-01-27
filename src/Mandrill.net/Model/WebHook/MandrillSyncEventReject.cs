using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillSyncEventReject
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public string Detail { get; set; }

        public string Email { get; set; }

        public bool? Expired { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? ExpiresAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? LastEventAt { get; set; }

        public string Reason { get; set; }

        public string Sender { get; set; }

        public string Subaccount { get; set; }
    }
}
