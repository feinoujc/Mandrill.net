using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillMessageSearchRequest : MandrillRequestBase
    {
        public string Query { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public IList<string> Tags { get; set; }

        public IList<string> Senders { get; set; }

        public IList<string> ApiKeys { get; set; }

        public int? Limit { get; set; }
    }
}