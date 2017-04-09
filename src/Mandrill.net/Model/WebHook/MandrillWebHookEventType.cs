using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillWebHookEventType
    {
        Send,
        [EnumMember(Value = "hard_bounce")]
        HardBounce,
        [EnumMember(Value = "soft_bounce")]
        SoftBounce,
        Open,
        Click,
        Spam,
        Unsub,
        Reject,
        Blacklist,
        Whitelist
    }
}
