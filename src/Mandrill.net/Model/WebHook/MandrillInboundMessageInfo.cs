using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillInboundMessageInfo
    {
        public MandrillDkimInfo Dkim { get; set; }

        public string Email { get; set; }

        public string FromEmail { get; set; }

        public Dictionary<string, object> Headers { get; set; }

        public string Html { get; set; }

        public string RawMsg { get; set; }

        public object Sender { get; set; }

        public MandrillInboundSpamReport SpamReport { get; set; }


        public MandrillInboundSpfInfo Spf { get; set; }

        public string Subject { get; set; }

        public IList<string> Tags { get; set; }

        public string Template { get; set; }

        public string Text { get; set; }

        public bool TextFlowed { get; set; }

        public List<List<string>> To { get; set; }
    }
}