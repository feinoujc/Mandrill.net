using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
namespace Mandrill.Model
{
    public class MandrillTagTimeSeries : MandrillTagInfo
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
