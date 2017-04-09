using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    internal class MandrillInboundRequest : MandrillRequestBase
    {
        public string Domain { get; set; }
    }
}
