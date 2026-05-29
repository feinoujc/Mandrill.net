using System;
using Mandrill.Serialization;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSenderDemographics : MandrillAggregateStatisticsBase
    {
        public string Address { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
