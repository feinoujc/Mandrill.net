using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillInboundAttachment
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        [JsonProperty("base64")]
        public bool Base64 { get; set; }
    }
}
