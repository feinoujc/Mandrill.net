using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillSendRawMessageRequest : MandrillRequestBase
    {
        public string RawMessage { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public List<string> To { get; set; } = new List<string>();
        
        public bool? Async { get; set; }
        public string IpPool { get; set; }
        public string SendAt { get; set; }
        public string ReturnPathDomain { get; set; }
    }
}