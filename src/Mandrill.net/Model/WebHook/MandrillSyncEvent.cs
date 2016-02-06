using System;
using System.Collections.Generic;
using System.IO;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillSyncEvent
    {
        private Dictionary<string, string> _reject;
        
        public MandrillSyncType Type { get; set; }
        
        public MandrillSyncAction Action { get; set; }

        public Dictionary<string, string> Reject
        {
            get
            {
                return this._reject ?? (this._reject = new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase));
            }
            set
            {
                this._reject = value;
            }
        }

        public static List<MandrillSyncEvent> ParseSyncEvents(string json)
        {
            using (var jsonTextReader = new JsonTextReader(new StringReader(json)))
                return MandrillSerializer<List<MandrillSyncEvent>>.Deserialize(jsonTextReader);
        }
    }
}
