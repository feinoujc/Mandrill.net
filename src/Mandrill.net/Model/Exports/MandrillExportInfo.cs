using Mandrill.Serialization;
using System.Text.Json.Serialization;
using System;

namespace Mandrill.Model
{
    public class MandrillExportInfo
    {
        public string Id { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public MandrillExportType Type { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? FinishedAt { get; set; }

        public MandrillExportState State { get; set; }

        public string ResultUrl { get; set; }
    }
}
