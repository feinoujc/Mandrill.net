
namespace Mandrill.Model
{
    internal class MandrillSenderVerifyDomainRequest : MandrillRequestBase
    {
        public string Domain { get; set; }
        public string Mailbox { get; set; }
    }
}
