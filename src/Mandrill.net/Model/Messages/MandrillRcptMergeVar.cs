using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillRcptMergeVar
    {
        private List<MandrillMergeVar> _vars;

        public string Rcpt { get; set; }

        public List<MandrillMergeVar> Vars
        {
            get { return _vars ?? (_vars = new List<MandrillMergeVar>()); }
            set { _vars = value; }
        }
    }
}