using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill.Model
{
    public class MandrillSenderInfo : MandrillAggregateStatisticsBase
    {
        public string Address { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public MandrillStats Stats { get; set; }
    }
}
