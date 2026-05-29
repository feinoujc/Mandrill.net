using Mandrill.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillMcTemplateTimeSeries : MandrillAggregateStatisticsBase
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
