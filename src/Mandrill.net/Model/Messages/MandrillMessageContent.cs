using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillMessageContent
    {
        public DateTime Ts { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public MandrillMailAddress To { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        [JsonConverter(typeof(EmptyArrayOrDictionaryConverter))]
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public string Text { get; set; }
        public string Html { get; set; }
        public List<MandrillAttachment> Attachments { get; set; } = new List<MandrillAttachment>();
    }
}
