namespace Mandrill.Model
{
    internal class MandrillSubaccountsRequest : MandrillRequestBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? CustomQuota { get; set; }
        public string Q { get; set; }
    }
}