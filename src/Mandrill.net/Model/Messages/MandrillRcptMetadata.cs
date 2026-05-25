using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mandrill.Serialization;

namespace Mandrill.Model
{
    public class MandrillRcptMetadata
    {
        public string Rcpt { get; set; }

        [JsonConverter(typeof(EmptyArrayOrDictionaryConverter))]
        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}
