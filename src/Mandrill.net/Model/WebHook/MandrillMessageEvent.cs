using System;
using System.Collections.Generic;
using System.IO;
using Mandrill.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [JsonProperty("_id")]
        public string Id { get; set; }

        public MandrillMessageEventInfo Msg { get; set; }

        public static List<MandrillMessageEvent> ParseMandrillEvents(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                return MandrillSerializer<List<MandrillMessageEvent>>.Deserialize(reader);
            }
        }
    }
}
