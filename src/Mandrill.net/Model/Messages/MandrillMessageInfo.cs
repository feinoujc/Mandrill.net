using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillMessageInfo
    {
        private List<MandrillClicksDetail> _clicksDetail;
        private Dictionary<string, string> _metadata;
        private List<MandrillOpensDetail> _opensDetail;
        private List<MandrillSmtpEvent> _smtpEvents;
        private List<string> _tags;
        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        public string Sender { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public List<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public int? Opens { get; set; }

        public List<MandrillOpensDetail> OpensDetail
        {
            get { return _opensDetail ?? (_opensDetail = new List<MandrillOpensDetail>()); }
            set { _opensDetail = value; }
        }

        public int? Clicks { get; set; }

        public List<MandrillClicksDetail> ClicksDetail
        {
            get { return _clicksDetail ?? (_clicksDetail = new List<MandrillClicksDetail>()); }
            set { _clicksDetail = value; }
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

    }
}