namespace Mandrill.Model
{
    internal class MandrillIpSetCustomDnsRequest : MandrillRequestBase
    {
        public string Ip { get; set; }
        public string Domain { get; set; }
    }
}
