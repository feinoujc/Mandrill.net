using System;

namespace Mandrill.Model
{
    public class MandrillSmtpEvent
    {
        public DateTime Ts { get; set; }

        public string Type { get; set; }

        public string Diag { get; set; }
    }
}