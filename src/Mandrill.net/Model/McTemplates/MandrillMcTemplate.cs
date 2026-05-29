using Mandrill.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillMcTemplate
    {
        public int McTemplateId { get; set; }
        public string McTemplateName { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public string Code { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Text { get; set; }
        public string PublishCode { get; set; }
        public string PublishText { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? PublishedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? DraftUpdatedAt { get; set; }

        public bool IsBrokenTemplate { get; set; }
    }
}
