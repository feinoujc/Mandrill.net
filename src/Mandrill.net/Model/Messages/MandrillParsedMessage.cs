using Mandrill.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillParsedMessage
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
        public bool TextFlowed { get; set; }

        [JsonConverter(typeof(EmptyArrayOrDictionaryConverter))]
        public Dictionary<string, MandrillHeaderValue> Headers { get; set; } = new Dictionary<string, MandrillHeaderValue>(System.StringComparer.OrdinalIgnoreCase);

        public List<MandrillMailAddress> To { get; set; } = new List<MandrillMailAddress>();
        public List<MandrillParsedAttachment> Attachments { get; set; } = new List<MandrillParsedAttachment>();

        [JsonIgnore]
        public string ReplyTo => Headers.TryGetValue("Reply-To", out var value) ? value.Value : null;
    }
}
