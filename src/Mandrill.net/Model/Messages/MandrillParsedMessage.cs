using Mandrill.Serialization;
using System.Collections.Generic;
using System.Text.Json;
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
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(System.StringComparer.OrdinalIgnoreCase);

        public List<MandrillMailAddress> To { get; set; } = new List<MandrillMailAddress>();
        public List<MandrillParsedAttachment> Attachments { get; set; } = new List<MandrillParsedAttachment>();

        [JsonIgnore]
        public string ReplyTo
        {
            get
            {
                if (Headers.TryGetValue("Reply-To", out var value))
                {
                    if (value is string text) return text;
                    if (value is JsonElement element && element.ValueKind == JsonValueKind.String)
                        return element.GetString();
                }
                return null;
            }
        }
    }
}
