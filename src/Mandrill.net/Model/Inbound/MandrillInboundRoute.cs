using System;

namespace Mandrill.Model
{
    public class MandrillInboundRoute
    {
        public string Id { get; set; }

        public string Pattern { get; set; }

        public Uri Url { get; set; }
    }
}
