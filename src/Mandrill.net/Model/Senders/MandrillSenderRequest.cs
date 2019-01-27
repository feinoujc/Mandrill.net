namespace Mandrill.Model
{
    internal class MandrillSenderRequest : MandrillRequestBase
    {
        public string Domain { get; set; }
        public string Address { get; set; }
    }
}
