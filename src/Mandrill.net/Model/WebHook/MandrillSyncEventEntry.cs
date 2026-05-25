using System;
using Mandrill.Serialization;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSyncEventEntry
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public string Detail { get; set; }

        public string Email { get; set; }
    }
}
