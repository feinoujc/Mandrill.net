using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill.Model
{
    public class MandrillTagInfo : MandrillAggregateStatisticsBase
    {
        public string Tag { get; set; }

        public int Reputation { get; set; }

        public MandrillStats Stats { get; set; }
    }
}
