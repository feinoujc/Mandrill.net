using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillSenderDemographics : MandrillAggregateStatisticsBase
    {
        public string Address { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
