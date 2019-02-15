using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillMessageTimeSeries : MandrillAggregateStatisticsBase
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
