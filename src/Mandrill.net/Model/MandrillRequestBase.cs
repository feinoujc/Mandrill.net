using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    abstract class MandrillRequestBase
    {
        [JsonInclude]
        internal string Key { get; set; }
    }
}
