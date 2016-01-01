using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillInboundMessageInfo
    {
        private List<string> _tags;
        private List<List<string>> _to;
        private Dictionary<string, object> _headers;
        public MandrillDkimInfo Dkim { get; set; }

        public string Email { get; set; }

        public string FromEmail { get; set; }

        public Dictionary<string, object> Headers
        {
            get { return _headers ?? (_headers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)); }
            set { _headers = value; }
        }

        public string Html { get; set; }

        public string RawMsg { get; set; }

        public object Sender { get; set; }

        public MandrillInboundSpamReport SpamReport { get; set; }


        public MandrillInboundSpfInfo Spf { get; set; }

        public string Subject { get; set; }

        public List<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public string Template { get; set; }

        public string Text { get; set; }

        public bool TextFlowed { get; set; }

        public List<List<string>> To
        {
            get { return _to ?? (_to = new List<List<string>>()); }
            set { _to = value; }
        }
    }
}