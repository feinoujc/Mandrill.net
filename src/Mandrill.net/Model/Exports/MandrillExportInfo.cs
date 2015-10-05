using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mandrill.Model
{
    public class MandrillExportInfo
    {
        public string Id { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime FinishedAt { get; set; }

        public string State { get; set; }

        public string ResultUrl { get; set; }
    }
}
