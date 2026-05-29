using Mandrill.Serialization;
using System.Text.Json.Serialization;
using System;
namespace Mandrill.Model
{
    public class MandrillTagTimeSeries : MandrillTagInfo
    {
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }
    }
}
