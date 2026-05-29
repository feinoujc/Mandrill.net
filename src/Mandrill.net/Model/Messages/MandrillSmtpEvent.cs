using System;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillSmtpEvent
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Ts { get; set; }

        public string Type { get; set; }
        public string Diag { get; set; }
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public int? Size { get; set; }
    }
}
