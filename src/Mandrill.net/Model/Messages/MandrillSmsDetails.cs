using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillSmsDetails
    {
        public string Text { get; set; }
        public List<string> To { get; set; } = new List<string>();
        public string From { get; set; }
        public MandrillSmsDetailsConsent Consent { get; set; }
        public bool? TrackClicks { get; set; }
    }
}
