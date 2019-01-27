using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillMessageEventType
    {
        Send,
        Deferral,
        HardBounce,
        SoftBounce,
        Open,
        Click,
        Spam,
        Unsub,
        Reject
    }
}
