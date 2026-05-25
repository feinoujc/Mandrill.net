using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSendMessageResponse
    {
        public string Email { get; set; }
        public MandrillSendMessageResponseStatus Status { get; set; }
        public string RejectReason { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }
}
