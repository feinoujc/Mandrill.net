using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillInboundAttachment
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }

        [JsonPropertyName("base64")]
        public bool Base64 { get; set; }
    }
}
