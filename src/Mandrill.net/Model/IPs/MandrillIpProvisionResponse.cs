using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillIpProvisionResponse
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime RequestedAt { get; set; }
    }
}
