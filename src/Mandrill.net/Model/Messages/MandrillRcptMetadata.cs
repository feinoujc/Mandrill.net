using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillRcptMetadata
    {
        private Dictionary<string, string> _values;

        public string Rcpt { get; set; }

        public Dictionary<string, string> Values
        {
            get { return _values ?? (_values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _values = value; }
        }
    }
}