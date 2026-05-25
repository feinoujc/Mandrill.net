using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    [JsonConverter(typeof(MandrillMessageStateConverter))]
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
