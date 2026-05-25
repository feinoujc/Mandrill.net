using System;
using Mandrill.Serialization;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillMessageTimeSeries : MandrillAggregateStatisticsBase
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
