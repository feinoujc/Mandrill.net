using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mandrill.Model
{
    public class MandrillWhitelistInfo
    {
        public string Email { get; set; }

        public string Detail { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public bool Added { get; set; }

        public bool Deleted { get; set; }
    }
}
