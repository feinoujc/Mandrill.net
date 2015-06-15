using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    class MandrillWebHookRequest : MandrillRequestBase
    {
        public Uri Url { get; set; }
        public string Description { get; set; }
        public IList<MandrillWebHookEventType> Events { get; set; }
        public int? Id { get; set; }
    }
}