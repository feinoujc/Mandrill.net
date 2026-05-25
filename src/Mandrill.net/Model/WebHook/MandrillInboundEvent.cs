using System;
using System.Collections.Generic;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillInboundEvent
    {
        public MandrillInboundEventType Event { get; set; }
        public MandrillInboundMessageInfo Msg { get; set; }
        public DateTime Ts { get; set; }

        public static List<MandrillInboundEvent> ParseMandrillEvents(string json)
            => MandrillSerializer<List<MandrillInboundEvent>>.Deserialize(json);
    }
}
