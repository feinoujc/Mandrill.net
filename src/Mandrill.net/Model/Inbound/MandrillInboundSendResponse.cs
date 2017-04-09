using System;

namespace Mandrill.Model
{
    public class MandrillInboundSendResponse
    {
        public string Email { get; set; }

        public string Pattern { get; set; }

        public Uri Url { get; set; }
    }
}
