using System;
using System.Collections.Generic;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillSyncEvent
    {
        public DateTime Ts { get; set; }
        public MandrillSyncType Type { get; set; }
        public MandrillSyncAction Action { get; set; }
        public MandrillSyncEventReject Reject { get; set; }
        public MandrillSyncEventEntry Entry { get; set; }

        public static List<MandrillSyncEvent> ParseSyncEvents(string json)
            => MandrillSerializer<List<MandrillSyncEvent>>.Deserialize(json);
    }
}
