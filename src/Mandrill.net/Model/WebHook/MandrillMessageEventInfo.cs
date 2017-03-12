using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillMessageEventInfo
    {
        public DateTime? Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_version")]
        public string Version { get; set; }

        public string Sender { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public List<MandrillOpensDetail> Opens { get; set; } = new List<MandrillOpensDetail>();

        public List<MandrillClicksDetail> Clicks { get; set; } = new List<MandrillClicksDetail>();

        public MandrillMessageState State { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public List<MandrillSmtpEvent> SmtpEvents { get; set; } = new List<MandrillSmtpEvent>();

        public string Subaccount { get; set; }

        public string Diag { get; set; }

        public string BounceDescription { get; set; }

        [JsonProperty("bgtools_code")]
        public int? BgToolsCode { get; set; }
    }
}