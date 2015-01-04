namespace Mandrill.Model
{
    internal class MandrillRejectRequest : MandrillRequestBase
    {
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Subaccount { get; set; }
        public bool? IncludeExpired { get; set; }
    }
}