using System;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillMessageScheduleInfo
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime SendAt { get; set; }

        public string FromEmail { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}
