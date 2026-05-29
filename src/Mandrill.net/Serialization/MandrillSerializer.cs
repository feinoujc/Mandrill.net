using System;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Mandrill.Serialization
{
    public static class MandrillSerializer
    {
        public static JsonSerializerOptions Instance { get; } = Create();

        internal static JsonSerializerOptions CreateContentOptions()
        {
            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            };
            opts.Converters.Add(new UnixDateTimeConverter());
            opts.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: false));
            return opts;
        }

        private static JsonSerializerOptions Create()
        {
            var contentOptions = CreateContentOptions();
            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver().WithAddedModifier(SkipEmptyCollectionsModifier),
            };
            opts.Converters.Add(new UnixDateTimeConverter());
            opts.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: false));
            opts.Converters.Add(new MandrillMergeVarConverter(contentOptions));
            return opts;
        }

        private static void SkipEmptyCollectionsModifier(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return;
            }

            foreach (var prop in typeInfo.Properties)
            {
                var memberInfo = prop.AttributeProvider as MemberInfo;
                var shouldSerializeMethod = memberInfo?.DeclaringType?.GetMethod(
                    $"ShouldSerialize{memberInfo.Name}",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);

                if (shouldSerializeMethod?.ReturnType == typeof(bool))
                {
                    prop.ShouldSerialize = (instance, _) => (bool)shouldSerializeMethod.Invoke(instance, null)!;
                    continue;
                }

                if (prop.ShouldSerialize != null)
                {
                    continue;
                }

                if (IsCollectionType(prop.PropertyType))
                {
                    prop.ShouldSerialize = (_, value) => value is ICollection collection ? collection.Count > 0 : value != null;
                }
            }
        }

        private static bool IsCollectionType(Type type)
        {
            if (type == typeof(string) || !type.IsGenericType)
            {
                return false;
            }

            var genericType = type.GetGenericTypeDefinition();
            if (genericType == typeof(System.Collections.Generic.IList<>) ||
                genericType == typeof(System.Collections.Generic.List<>) ||
                genericType == typeof(System.Collections.Generic.IDictionary<,>) ||
                genericType == typeof(System.Collections.Generic.Dictionary<,>))
            {
                return true;
            }

            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType)
                {
                    continue;
                }

                var ifaceType = iface.GetGenericTypeDefinition();
                if (ifaceType == typeof(System.Collections.Generic.IList<>) || ifaceType == typeof(System.Collections.Generic.IDictionary<,>))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class MandrillSerializer<T>
    {
        public static T Deserialize(string json)
            => JsonSerializer.Deserialize<T>(json, MandrillSerializer.Instance)!;

        public static string Serialize(T value)
            => JsonSerializer.Serialize(value, MandrillSerializer.Instance);
    }
}
