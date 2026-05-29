using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillStats
    {
        public MandrillAggregateStatisticsBase Today { get; set; }

        [JsonPropertyName("last_7_days")]
        public MandrillAggregateStatisticsBase Last7Days { get; set; }

        [JsonPropertyName("last_30_days")]
        public MandrillAggregateStatisticsBase Last30Days { get; set; }

        [JsonPropertyName("last_60_days")]
        public MandrillAggregateStatisticsBase Last60Days { get; set; }

        [JsonPropertyName("last_90_days")]
        public MandrillAggregateStatisticsBase Last90Days { get; set; }

        public MandrillAggregateStatisticsBase AllTime { get; set; }
    }
}
