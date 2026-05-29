using Mandrill.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillIpPool
    {
        public string Name { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public List<MandrillIpInfo> Ips { get; set; } = new List<MandrillIpInfo>();
    }
}
