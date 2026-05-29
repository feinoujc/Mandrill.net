
namespace Mandrill.Model
{
    public enum MandrillMessageEventType
    {
        Send,
        Deferral,
        HardBounce,
        SoftBounce,
        Delivered,
        Open,
        Click,
        Spam,
        Unsub,
        Reject,
    }
}
