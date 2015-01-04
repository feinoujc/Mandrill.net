using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillMessageScheduleInfo
    {
        [JsonProperty("_id")]
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