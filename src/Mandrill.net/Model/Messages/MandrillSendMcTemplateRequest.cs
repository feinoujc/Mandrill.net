namespace Mandrill.Model
{
    internal class MandrillSendMcTemplateRequest : MandrillSendMessageRequest
    {
        public int McTemplateId { get; set; }
        public MandrillMcTemplateVersion? McTemplateVersion { get; set; }
    }
}
