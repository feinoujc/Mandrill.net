using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class MandrillJsonContractResolver : DefaultContractResolver
    {
        public MandrillJsonContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy(false, false);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            var propertyType = jsonProperty.PropertyType;

            //don't serialize empty lists and dictionaries (unless overridden)
            if (jsonProperty.ShouldSerialize == null)
            {
                if (propertyType.GetTypeInfo().IsGenericType && propertyType.GenericTypeArguments.Length == 1)
                {
                    var t = propertyType.GenericTypeArguments[0];
                    if (typeof(IList<>).MakeGenericType(t).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
                    {
                        var prop = jsonProperty.DeclaringType.GetTypeInfo().GetDeclaredProperty(member.Name);
                        jsonProperty.ShouldSerialize = instance =>
                        {
                            var collection = prop.GetValue(instance) as ICollection;
                            return collection?.Count > 0;
                        };
                    }
                }

                if (typeof(IDictionary<string, string>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
                {
                    var prop = jsonProperty.DeclaringType.GetTypeInfo().GetDeclaredProperty(member.Name);

                    jsonProperty.ShouldSerialize = instance =>
                    {
                        var dictionary = prop.GetValue(instance) as IDictionary<string, string>;
                        return dictionary?.Count > 0;
                    };

                }

                if (typeof(IDictionary<string, object>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
                {
                    var prop = jsonProperty.DeclaringType.GetTypeInfo().GetDeclaredProperty(member.Name);

                    jsonProperty.ShouldSerialize = instance =>
                    {
                        var dictionary = prop.GetValue(instance) as IDictionary<string, object>;
                        return dictionary?.Count > 0;
                    };
                }
            }

            return jsonProperty;
        }
    }
}
