using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillInboundSpamReport
    {
        public IList<MandrillInboundMatchedRule> MatchedRules { get; set; }
        public double Score { get; set; }
    }
}
