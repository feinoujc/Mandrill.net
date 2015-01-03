using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillMessageInfo
    {
        private IList<MandrillClicksDetail> _clicksDetail;
        private IDictionary<string, string> _metadata;
        private IList<MandrillOpensDetail> _opensDetail;
        private IList<string> _tags;
        private IList<MandrillSmtpEvent> _smtpEvents;
        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public Guid Id { get; set; }

        public string Sender { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public IList<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public int? Opens { get; set; }

        public IList<MandrillOpensDetail> OpensDetail
        {
            get { return _opensDetail ?? (_opensDetail = new List<MandrillOpensDetail>()); }
            set { _opensDetail = value; }
        }

        public int? Clicks { get; set; }

        public IList<MandrillClicksDetail> ClicksDetail
        {
            get { return _clicksDetail ?? (_clicksDetail = new List<MandrillClicksDetail>()); }
            set { _clicksDetail = value; }
        }

        [JsonConverter(typeof (StringEnumConverter))]
        public MandrillMessageState State { get; set; }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, string>()); }
            set { _metadata = value; }
        }

        public IList<MandrillSmtpEvent> SmtpEvents
        {
            get { return _smtpEvents ?? (_smtpEvents = new List<MandrillSmtpEvent>()); }
            set { _smtpEvents = value; }
        }
    }
}