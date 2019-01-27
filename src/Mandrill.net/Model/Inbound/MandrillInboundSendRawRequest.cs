using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillInboundSendRawRequest : MandrillRequestBase
    {
        public string RawMessage { get; set; }
        public IList<string> To { get; set; }
        public string MailFrom { get; set; }
        public string Helo { get; set; }
        public string ClientAddress { get; set; }
    }
}
