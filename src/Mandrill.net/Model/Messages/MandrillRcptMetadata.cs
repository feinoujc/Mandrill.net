using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillRcptMetadata
    {
        public string Rcpt { get; set; }

        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}