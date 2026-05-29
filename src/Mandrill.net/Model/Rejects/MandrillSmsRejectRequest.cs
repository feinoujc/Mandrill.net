namespace Mandrill.Model
{
    internal class MandrillSmsRejectRequest : MandrillRequestBase
    {
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string Subaccount { get; set; }
        public bool? IncludeExpired { get; set; }
    }
}
