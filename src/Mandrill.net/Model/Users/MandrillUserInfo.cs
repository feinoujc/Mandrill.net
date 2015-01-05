using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillUserInfo
    {
        public string Username { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public string PublicId { get; set; }

        public int Reputation { get; set; }

        public int HourlyQuota { get; set; }

        public int Backlog { get; set; }

        public MandrillStats Stats { get; set; }
    }
}