using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillRcptMergeVar
    {
        public string Rcpt { get; set; }

        public List<MandrillMergeVar> Vars { get; set; } = new List<MandrillMergeVar>();
    }
}