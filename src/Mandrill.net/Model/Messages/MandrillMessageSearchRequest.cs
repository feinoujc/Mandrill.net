using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillMessageSearchRequest : MandrillRequestBase
    {
        public string Query { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Senders { get; set; }

        public List<string> ApiKeys { get; set; }

        public int? Limit { get; set; }
    }
}