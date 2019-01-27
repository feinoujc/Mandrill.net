namespace Mandrill.Model
{
    public class MandrillAggregateStatisticsBase
    {
        public int Sent { get; set; }

        public int HardBounces { get; set; }

        public int SoftBounces { get; set; }

        public int Rejects { get; set; }

        public int Complaints { get; set; }

        public int Unsubs { get; set; }

        public int Opens { get; set; }

        public int UniqueOpens { get; set; }

        public int Clicks { get; set; }

        public int UniqueClicks { get; set; }
    }
}
