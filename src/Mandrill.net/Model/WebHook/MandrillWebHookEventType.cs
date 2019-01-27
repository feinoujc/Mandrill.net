using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillWebHookEventType
    {
        Send,
        HardBounce,
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
