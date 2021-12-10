using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mandrill.Model
{
    public class MandrillSenderDomain
    {
        public string Domain { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastTestedAt { get; set; }
        public MandrillSenderValidDetails SPF { get; set; }
        public MandrillSenderValidDetails DKIM { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime VerifiedAt { get; set; }
        public bool ValidSigning { get; set; }
        public string VerifyTxtKey { get; set; }
        public MandrillSenderVerifyDomain Verified { get; set; }
    }
}
