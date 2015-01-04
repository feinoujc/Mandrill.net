namespace Mandrill.Model
{
    public class MandrillRejectDeleteResponse
    {
        public string Email { get; set; }
        public bool Deleted { get; set; }
        public string Subaccount { get; set; }
    }
}