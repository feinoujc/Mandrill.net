using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillSendMessageResponse
    {
        public string Email { get; set; }

        public MandrillSendMessageResponseStatus Status { get; set; }

        public string RejectReason { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}