namespace Mandrill.Model
{
    internal class MandrillIpSetPoolRequest : MandrillRequestBase
    {
        public string Ip { get; set; }
        public string Pool { get; set; }
        public bool? CreatePool { get; set; }
    }
}
