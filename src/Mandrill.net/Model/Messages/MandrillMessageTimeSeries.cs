using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillMessageTimeSeries : MandrillMessageAggregateStatisticsBase
    {
        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}