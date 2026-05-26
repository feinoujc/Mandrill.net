using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter<MandrillMessageState>))]
    public enum MandrillMessageState
    {
        Sent,
        Rejected,
        Spam,
        Unsub,
        Bounced,
        [JsonStringEnumMemberName("soft-bounced")]
        SoftBounced,
        Deferred,
        Inbound
    }
}
