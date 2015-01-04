using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillRcptMetadata
    {
        private IDictionary<string, string> _values;

        public string Rcpt { get; set; }

        public IDictionary<string, string> Values
        {
            get { return _values ?? (_values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _values = value; }
        }
    }
}