using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Serialization
{
    /// <summary>
    /// This converter will allow an empty array to be converted to an empty dictionary
    /// </summary>
    internal class EmptyArrayOrDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Dictionary<string, object>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    // discard
                }
                return existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
            }
            // default behavior
            existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
            serializer.Populate(reader, existingValue);
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("CanWrite is false");
        }

        public override bool CanWrite => false;
    }

}
