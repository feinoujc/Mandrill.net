namespace Mandrill.Model
{
    internal class MandrillIpProvisionRequest : MandrillRequestBase
    {
        public bool? Warmup { get; set; }
        public string Pool { get; set; }
    }
}
