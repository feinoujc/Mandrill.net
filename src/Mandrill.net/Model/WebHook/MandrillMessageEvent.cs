using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillMessageEvent
    {
        public MandrillMessageEventType Event { get; set; }
        public string Url { get; set; }
        public DateTime Ts { get; set; }
        public string UserAgent { get; set; }
        public MandrillEventUserAgentParsed UserAgentParsed { get; set; }
        public string Ip { get; set; }
        public MandrillEventLocation Location { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public MandrillMessageEventInfo Msg { get; set; }

        public static List<MandrillMessageEvent> ParseMandrillEvents(string json)
            => MandrillSerializer<List<MandrillMessageEvent>>.Deserialize(json);
    }
}
