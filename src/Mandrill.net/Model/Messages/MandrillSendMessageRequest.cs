namespace Mandrill.Model
{
    internal class MandrillSendMessageRequest : MandrillRequestBase
    {
        public MandrillMessage Message { get; set; }
        public bool? Async { get; set; }
        public string IpPool { get; set; }
        public string SendAt { get; set; }
    }
}