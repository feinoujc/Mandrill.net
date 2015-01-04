using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillMessageEventType
    {
        Send,
        Deferral,
        [EnumMember(Value = "hard_bounce")] HardBounce,
        [EnumMember(Value = "soft_bounce")] SoftBounce,
        Open,
        Click,
        Spam,
        Unsub,
        Reject
    }
}