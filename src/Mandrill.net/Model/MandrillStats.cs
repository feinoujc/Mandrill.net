using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillStats
    {
        public MandrillAggregateStatisticsBase Today { get; set; }

        [JsonProperty("last_7_days")]
        public MandrillAggregateStatisticsBase Last7Days { get; set; }

        [JsonProperty("last_30_days")]
        public MandrillAggregateStatisticsBase Last30Days { get; set; }

        [JsonProperty("last_60_days")]
        public MandrillAggregateStatisticsBase Last60Days { get; set; }

        [JsonProperty("last_90_days")]
        public MandrillAggregateStatisticsBase Last90Days { get; set; }

        public MandrillAggregateStatisticsBase AllTime { get; set; }
    }
}