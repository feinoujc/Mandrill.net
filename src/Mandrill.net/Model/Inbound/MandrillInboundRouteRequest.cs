using System;

namespace Mandrill.Model
{
    internal class MandrillInboundRouteRequest : MandrillRequestBase
    {
        public string Domain { get; set; }
        public string Pattern { get; set; }
        public Uri Url { get; set; }
        public string Id { get; set; }
    }
}
