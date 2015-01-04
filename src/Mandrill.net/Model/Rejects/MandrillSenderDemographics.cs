using System;

namespace Mandrill.Model
{
    public class MandrillSenderDemographics : MandrillMessageAggregateStatisticsBase
    {
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}