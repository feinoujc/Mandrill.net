using System;
using System.Collections.Generic;
using System.IO;
using Mandrill.Serialization;
using Newtonsoft.Json;

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
        {
            using (var jsonTextReader = new JsonTextReader(new StringReader(json)))
                return MandrillSerializer<List<MandrillSyncEvent>>.Deserialize(jsonTextReader);
        }
    }
}
