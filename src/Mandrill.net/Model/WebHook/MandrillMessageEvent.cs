using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Mandrill.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Mandrill.Model
{
    public enum MandrillMessageEventType
    {
        Send,
        Deferral,
        [EnumMember(Value = "hard_bounce")] HardBounce,
        [EnumMember(Value = "soft_bounce")] SoftBounce,
        Open,
        Click,
        Spam,
        Unsub,
        Reject
    }

    public class MandrillMessageEvent
    {
        [JsonConverter(typeof (StringEnumConverter))]
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

        public static IList<MandrillMessageEvent> ParseMandrillEvents(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                return JArray.Load(reader).ToObject<List<MandrillMessageEvent>>(MandrillSerializer.Instance);
            }
        }
    }
}