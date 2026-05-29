
namespace Mandrill.Model
{
    public enum MandrillWebHookEventType
    {
        Send,
        Delivered,
        Deferral,
        HardBounce,
        SoftBounce,
        Open,
        Click,
        Spam,
        Unsub,
        Reject,
        Blacklist,
        Whitelist,
        SmsProcessing,
        SmsQueued,
        SmsSent,
        SmsDelivered,
        SmsCanceled,
        SmsFailed,
        SmsOpen,
        SmsClick,
        SmsUnsub,
    }
}
