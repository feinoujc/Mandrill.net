using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSmsRejectInfo
    {
        public string Phone { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? ExpiresAt { get; set; }

        public bool Expired { get; set; }
        public string Subaccount { get; set; }
    }
}
