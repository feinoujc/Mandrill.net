using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillInboundMessageInfo
    {
        public MandrillDkimInfo Dkim { get; set; }

        public string Email { get; set; }

        public string FromEmail { get; set; }

        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public string Html { get; set; }

        public string RawMsg { get; set; }

        public object Sender { get; set; }

        public MandrillInboundSpamReport SpamReport { get; set; }

        public Dictionary<string, MandrillInboundAttachment> Attachments { get; set; } = new Dictionary<string, MandrillInboundAttachment>();

        public Dictionary<string, MandrillInboundImage> Images { get; set; } = new Dictionary<string, MandrillInboundImage>();

        public MandrillInboundSpfInfo Spf { get; set; }

        public string Subject { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string Template { get; set; }

        public string Text { get; set; }

        public bool TextFlowed { get; set; }

        public List<List<string>> To { get; set; } = new List<List<string>>();
        
        public List<List<string>> CC { get; set; } = new List<List<string>>();
    }
}
