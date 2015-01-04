using System.Runtime.Serialization;

namespace Mandrill.Model
{
    public enum MandrillMailAddressType
    {
        [EnumMember(Value = "to")] To,
        [EnumMember(Value = "cc")] Cc,
        [EnumMember(Value = "bcc")] Bcc
    }
}