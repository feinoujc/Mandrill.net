using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillExportRequest : MandrillRequestBase
    {
        public string Id { get; set; }

        public string NotifyEmail { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Senders { get; set; }

        public List<string> States { get; set; }

        public List<string> ApiKeys { get; set; }

    }
}
