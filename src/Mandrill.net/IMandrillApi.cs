namespace Mandrill
{
    public interface IMandrillApi
    {
        IMandrillMessagesApi Messages { get; }
        IMandrillTagsApi Tags { get; }
        IMandrillTemplatesApi Templates { get; }
        IMandrillRejectsApi Rejects { get; }
        IMandrillUsersApi Users { get; }
        IMandrillSendersApi Senders { get; }
        IMandrillAllowlistsApi Allowlists { get; }
        IMandrillSubaccountsApi Subaccounts { get; }
        IMandrillInboundApi Inbound { get; }
        IMandrillWebHooksApi WebHooks { get; }
        IMandrillExportsApi Exports { get; }
        IMandrillIpsApi Ips { get; }
        IMandrillMcTemplatesApi McTemplates { get; }
        IMandrillMetadataApi Metadata { get; }
        IMandrillUrlsApi Urls { get; }
    }
}
