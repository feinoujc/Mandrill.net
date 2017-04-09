using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillSubaccountResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CustomQuota { get; set; }

        public MandrillSubaccountStatus Status { get; set; }

        public int Reputation { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime? FirstSentAt { get; set; }

        public int SentWeekly { get; set; }

        public int SentMonthly { get; set; }

        public int SentTotal { get; set; }
    }
}
