using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    public class MandrillPing2Response
    {
        [JsonPropertyName("PING")]
        public string Ping { get; set; }
    }
}
