using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillMessageEventInfo
    {
        private List<MandrillClicksDetail> _clicks;
        private Dictionary<string, string> _metadata;
        private List<MandrillOpensDetail> _opens;
        private List<MandrillSmtpEvent> _smtpEvents;
        private List<string> _tags;
        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_version")]
        public string Version { get; set; }

        public string Sender { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public List<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public List<MandrillOpensDetail> Opens
        {
            get { return _opens ?? (_opens = new List<MandrillOpensDetail>()); }
            set { _opens = value; }
        }

        public List<MandrillClicksDetail> Clicks
        {
            get { return _clicks ?? (_clicks = new List<MandrillClicksDetail>()); }
            set { _clicks = value; }
        }

        public MandrillMessageState State { get; set; }

        public Dictionary<string, string> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _metadata = value; }
        }

        public List<MandrillSmtpEvent> SmtpEvents
        {
            get { return _smtpEvents ?? (_smtpEvents = new List<MandrillSmtpEvent>()); }
            set { _smtpEvents = value; }
        }

        public string Subaccount { get; set; }

        public string Diag { get; set; }

        public string BounceDescription { get; set; }
    }
}