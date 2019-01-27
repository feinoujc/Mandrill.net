using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillWebHookInfo
    {
        public int Id { get; set; }

        public Uri Url { get; set; }

        public string Description { get; set; }

        public string AuthKey { get; set; }

        public List<MandrillWebHookEventType> Events { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }


        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime? LastSentAt { get; set; }

        public int BatchesSent { get; set; }

        public int EventsSent { get; set; }

        public string LastError { get; set; }
    }
}
