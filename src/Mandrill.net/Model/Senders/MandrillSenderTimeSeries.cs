using Mandrill.Serialization;
using System.Text.Json.Serialization;
using System;

namespace Mandrill.Model
{
    public class MandrillSenderTimeSeries : MandrillSenderInfo
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
