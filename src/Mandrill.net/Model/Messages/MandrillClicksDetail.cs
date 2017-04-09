using System;

namespace Mandrill.Model
{
    public class MandrillClicksDetail
    {
        public DateTime Ts { get; set; }

        public string Url { get; set; }

        public string Ip { get; set; }

        public string Location { get; set; }

        public string Ua { get; set; }
    }
}
