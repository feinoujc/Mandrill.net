using System;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillOpensDetail
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Ts { get; set; }

        public string Ip { get; set; }
        public string Location { get; set; }
        public string Ua { get; set; }
    }
}
