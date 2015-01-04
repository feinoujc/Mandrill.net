using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillMessageEventInfo
    {
        private IList<MandrillClicksDetail> _clicks;
        private IDictionary<string, string> _metadata;
        private IList<MandrillOpensDetail> _opens;
        private IList<MandrillSmtpEvent> _smtpEvents;
        private IList<string> _tags;
        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_version")]
        public string Version { get; set; }

        public string Sender { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public IList<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public IList<MandrillOpensDetail> Opens
        {
            get { return _opens ?? (_opens = new List<MandrillOpensDetail>()); }
            set { _opens = value; }
        }

        public IList<MandrillClicksDetail> Clicks
        {
            get { return _clicks ?? (_clicks = new List<MandrillClicksDetail>()); }
            set { _clicks = value; }
        }

        [JsonConverter(typeof (StringEnumConverter))]
        public MandrillMessageState State { get; set; }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _metadata = value; }
        }

        public IList<MandrillSmtpEvent> SmtpEvents
        {
            get { return _smtpEvents ?? (_smtpEvents = new List<MandrillSmtpEvent>()); }
            set { _smtpEvents = value; }
        }

        public string Subaccount { get; set; }

        public string Diag { get; set; }

        public string BounceDescription { get; set; }
    }
}