namespace Mandrill.Model
{
    public class MandrillSmsReject
    {
        public string Phone { get; set; }
        public bool? Added { get; set; }
        public bool? Deleted { get; set; }
        public string Subaccount { get; set; }
    }
}
