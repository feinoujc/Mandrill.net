using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillMessageState
    {
        Sent,
        Rejected,
        Spam,
        Unsub,
        Bounced,
        [EnumMember(Value = "soft-bounced")]
        SoftBounced,
        Deferred,
        Inbound
    }

}
