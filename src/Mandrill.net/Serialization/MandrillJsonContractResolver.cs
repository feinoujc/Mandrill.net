using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class MandrillJsonContractResolver : DefaultContractResolver
    {
        private static readonly Regex UnderscoreCamelCaseRegex = new Regex("(?<!(^|[A-Z]))(?=[A-Z])|(?<!^)(?=[A-Z][a-z])", RegexOptions.Compiled);
        private readonly GuidConverter _guidConverter;
        private readonly UnixDateTimeConverter _unixDateTimeConverter;


        public MandrillJsonContractResolver(UnixDateTimeConverter unixDateTimeConverter, GuidConverter guidConverter)
        {
            _unixDateTimeConverter = unixDateTimeConverter;
            _guidConverter = guidConverter;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            if (UnderscoreCamelCaseRegex.IsMatch(propertyName))
            {
                return UnderscoreCamelCaseRegex.Replace(propertyName, "_$1")
                    .TrimStart('_')
                    .ToLowerInvariant();
            }
            return propertyName.ToLowerInvariant();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            var propertyType = jsonProperty.PropertyType;

            //don't serialize empty lists (unless required)
            if (propertyType.IsGenericType && propertyType.GenericTypeArguments.Length == 1)
            {
                if (member.GetCustomAttribute<RequiredAttribute>() == null)
                {
                    var t = propertyType.GenericTypeArguments[0];
                    if (typeof (IList<>).MakeGenericType(t).IsAssignableFrom(propertyType))
                    {
                        jsonProperty.ShouldSerialize = instance =>
                        {
                            var collection = jsonProperty.DeclaringType.GetProperty(member.Name).GetValue(instance) as ICollection;
                            if (collection != null)
                            {
                                return collection.Count > 0;
                            }
                            return false;
                        };
                    }
                }
            }


            //don't serialize empty dictionaries (unless required)
            if (typeof (IDictionary<string, string>).IsAssignableFrom(propertyType))
            {
                if (member.GetCustomAttribute<RequiredAttribute>() == null)
                {
                    jsonProperty.ShouldSerialize = instance =>
                    {
                        var dictionary = jsonProperty.DeclaringType.GetProperty(member.Name).GetValue(instance) as IDictionary<string, string>;
                        if (dictionary != null)
                        {
                            return dictionary.Count > 0;
                        }
                        return false;
                    };
                }
            }

            if (propertyType == typeof (DateTime) || propertyType == typeof (DateTime?)
                || propertyType == typeof (DateTimeOffset) || propertyType == typeof (DateTimeOffset?))
            {
                jsonProperty.Converter = _unixDateTimeConverter;
            }

            if (propertyType == typeof (Guid) || propertyType == typeof (Guid?))
            {
                jsonProperty.Converter = _guidConverter;
            }
            return jsonProperty;
        }
    }
}