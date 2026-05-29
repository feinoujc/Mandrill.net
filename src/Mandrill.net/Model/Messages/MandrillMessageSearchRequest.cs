using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillMessageSearchRequest : MandrillRequestBase
    {
        public string Query { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Senders { get; set; }

        public List<string> ApiKeys { get; set; }

        public int? Limit { get; set; }
    }
}
