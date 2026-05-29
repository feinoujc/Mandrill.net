using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillSmsMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public MandrillSendMessageResponseStatus Status { get; set; }
        public string RejectReason { get; set; }
        public string QueueReason { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }
}
