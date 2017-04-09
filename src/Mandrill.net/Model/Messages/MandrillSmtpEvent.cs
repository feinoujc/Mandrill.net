using System;

namespace Mandrill.Model
{
    public class MandrillSmtpEvent
    {
        public DateTime Ts { get; set; }

        public string Type { get; set; }

        public string Diag { get; set; }

        public string SourceIp { get; set; }

        public string DestinationIp { get; set; }

        public int? Size { get; set; }
    }
}
