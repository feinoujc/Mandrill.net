using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mandrill.Model
{
    public class MandrillSenderTimeSeries : MandrillSenderInfo
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
