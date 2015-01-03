using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillSendMessageResponse
    {
        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MandrillSendMessageResponseStatus Status { get; set; }

        public string RejectReason { get; set; }

        [JsonProperty("_id")]
        public Guid Id { get; set; }
    }
}