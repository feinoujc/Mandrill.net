using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillSubaccountInfo : MandrillSubaccountResponse
    {
        [JsonProperty("last_30_days")]
        public MandrillSubaccountStats Last30Days { get; set; }
    }
}