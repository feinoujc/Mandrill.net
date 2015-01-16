using System;
using System.Collections.Generic;
using System.IO;
using Mandrill.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mandrill.Model
{
    public class MandrillInboundEvent
    {
        public MandrillInboundEventType Event { get; set; }

        public MandrillInboundMessageInfo Msg { get; set; }

        public DateTime Ts { get; set; }

        public static List<MandrillInboundEvent> ParseMandrillEvents(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                return JArray.Load(reader).ToObject<List<MandrillInboundEvent>>(MandrillSerializer.Instance);
            }
        }
    }
}