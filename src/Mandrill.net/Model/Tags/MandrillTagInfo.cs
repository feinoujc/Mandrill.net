using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill.Model
{
    public class MandrillTagInfo
    {
        public string Tag { get; set; }

        public int Reputation { get; set; }

        public int Sent { get; set; }

        public int HardBounces { get; set; }

        public int SoftBounces { get; set; }

        public int Rejects { get; set; }

        public int Complaints { get; set; }

        public int Unsubs { get; set; }

        public int Opens { get; set; }

        public int Clicks { get; set; }

        public int UniqueOpens { get; set; }

        public int UniqueClicks { get; set; }

        public MandrillStats Stats { get; set; }
    }
}
