using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillSendSmsRequest : MandrillRequestBase
    {
        public MandrillSmsMessageObject Message { get; set; }
        public bool? Async { get; set; }
    }

    internal class MandrillSmsMessageObject
    {
        public MandrillSmsDetails Sms { get; set; }
        public bool? Merge { get; set; }
        public MandrillMessageMergeLanguage? MergeLanguage { get; set; }
        public List<MandrillMergeVar> GlobalMergeVars { get; set; }
        public List<MandrillRcptMergeVar> MergeVars { get; set; }
    }
}
