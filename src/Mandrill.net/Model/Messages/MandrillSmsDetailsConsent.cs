using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public enum MandrillSmsDetailsConsent
    {
        Onetime,
        Recurring,
        [JsonStringEnumMemberName("recurring-no-confirm")]
        RecurringNoConfirm
    }
}
