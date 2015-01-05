using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillStats
    {
        public MandrillUserStats Today { get; set; }

        [JsonProperty("last_7_days")]
        public MandrillUserStats Last7Days { get; set; }

        [JsonProperty("last_30_days")]
        public MandrillUserStats Last30Days { get; set; }

        [JsonProperty("last_60_days")]
        public MandrillUserStats Last60Days { get; set; }

        [JsonProperty("last_90_days")]
        public MandrillUserStats Last90Days { get; set; }

        public MandrillUserStats AllTime { get; set; }
    }
}