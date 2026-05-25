using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mandrill.Model;

namespace Mandrill.Serialization
{
    internal class MandrillMessageStateConverter : JsonConverter<MandrillMessageState>
    {
        public override MandrillMessageState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "sent" => MandrillMessageState.Sent,
                "rejected" => MandrillMessageState.Rejected,
                "spam" => MandrillMessageState.Spam,
                "unsub" => MandrillMessageState.Unsub,
                "bounced" => MandrillMessageState.Bounced,
                "soft-bounced" => MandrillMessageState.SoftBounced,
                "soft_bounced" => MandrillMessageState.SoftBounced,
                "deferred" => MandrillMessageState.Deferred,
                "inbound" => MandrillMessageState.Inbound,
                _ => throw new JsonException($"Unknown message state '{value}'.")
            };
        }

        public override void Write(Utf8JsonWriter writer, MandrillMessageState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value switch
            {
                MandrillMessageState.Sent => "sent",
                MandrillMessageState.Rejected => "rejected",
                MandrillMessageState.Spam => "spam",
                MandrillMessageState.Unsub => "unsub",
                MandrillMessageState.Bounced => "bounced",
                MandrillMessageState.SoftBounced => "soft-bounced",
                MandrillMessageState.Deferred => "deferred",
                MandrillMessageState.Inbound => "inbound",
                _ => throw new JsonException($"Unknown message state '{value}'.")
            });
        }
    }
}
