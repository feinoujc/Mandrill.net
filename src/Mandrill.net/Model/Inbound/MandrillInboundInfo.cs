using System;
using Mandrill.Serialization;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillInboundInfo
    {
        public string Domain { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public bool ValidMx { get; set; }
    }
}
