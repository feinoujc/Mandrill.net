using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSubaccountInfo : MandrillSubaccountResponse
    {
        [JsonPropertyName("last_30_days")]
        public MandrillSubaccountStats Last30Days { get; set; }
    }
}
