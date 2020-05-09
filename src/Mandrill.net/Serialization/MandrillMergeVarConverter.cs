using System;
using Mandrill.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mandrill.Serialization
{
    internal class MandrillMergeVarConverter : JsonConverter
    {
        private readonly JsonSerializer _contentSerializer;

        public MandrillMergeVarConverter(JsonSerializerSettings contentSettings)
        {
            _contentSerializer = JsonSerializer.Create(contentSettings);
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(MandrillMergeVar);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mergeVar = (MandrillMergeVar)value;

            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(mergeVar.Name);
            var content = JToken.FromObject((object)mergeVar.Content, _contentSerializer);
            writer.WritePropertyName("content");
            content.WriteTo(writer);
            writer.WriteEndObject();
        }
    }
}
