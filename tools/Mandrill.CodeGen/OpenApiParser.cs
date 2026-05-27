using System.Text.Json;

namespace Mandrill.CodeGen;

public class OpenApiParser
{
    private readonly NameMap _nameMap;
    private readonly JsonElement _root;
    private readonly JsonElement _schemas;
    private readonly JsonElement _paths;

    public OpenApiParser(string specPath, NameMap nameMap)
    {
        _nameMap = nameMap;
        var json = File.ReadAllText(specPath);
        _root = JsonDocument.Parse(json).RootElement;
        _schemas = _root.GetProperty("components").GetProperty("schemas");
        _paths = _root.GetProperty("paths");
    }

    public ApiModel Parse()
    {
        var endpoints = new List<(string tag, ApiEndpoint endpoint)>();

        foreach (var path in _paths.EnumerateObject())
        {
            var pathStr = path.Name;
            var post = path.Value.GetProperty("post");
            var operationId = post.GetProperty("operationId").GetString()!;
            var tags = post.GetProperty("tags").EnumerateArray().Select(t => t.GetString()!).ToList();
            var tag = tags.First().ToLowerInvariant();

            var requestSchemaName = GetRequestSchemaName(operationId);
            var responseSchemaName = GetResponseSchemaName(operationId);

            var requestSchema = requestSchemaName != null ? ParseRequestSchema(requestSchemaName) : null;
            var responseSchema = ParseResponseSchema(responseSchemaName);

            var isArrayResponse = responseSchema?.IsArray ?? false;
            string? arrayItemTypeName = null;

            if (isArrayResponse)
            {
                if (responseSchema?.ArrayItemRef != null)
                {
                    var itemSchemaName = responseSchema.ArrayItemRef.Split('/').Last();
                    arrayItemTypeName = MapSchemaName(itemSchemaName);
                }
                else
                {
                    arrayItemTypeName = MapSchemaName(responseSchemaName);
                }
            }

            var methodName = GetMethodName(operationId);
            var customImpl = IsCustomImpl(operationId);

            var endpoint = new ApiEndpoint(
                OperationId: operationId,
                Path: pathStr.TrimStart('/'),
                MethodName: methodName,
                RequestTypeName: MapSchemaName(requestSchemaName ?? operationId + "Request"),
                RequestSchema: requestSchema,
                ResponseTypeName: isArrayResponse ? arrayItemTypeName! : MapSchemaName(responseSchemaName),
                ResponseSchema: responseSchema,
                IsArrayResponse: isArrayResponse,
                ArrayItemTypeName: arrayItemTypeName,
                CustomImpl: customImpl
            );

            endpoints.Add((tag, endpoint));
        }

        var groups = endpoints
            .GroupBy(e => e.tag)
            .Select(g => BuildGroup(g.Key, g.Select(e => e.endpoint).ToList()))
            .OrderBy(g => g.Tag)
            .ToList();

        var sharedSchemas = ParseSharedSchemas();

        return new ApiModel(groups, sharedSchemas);
    }

    private Dictionary<string, SchemaModel> ParseSharedSchemas()
    {
        var result = new Dictionary<string, SchemaModel>();
        var sharedNames = new HashSet<string>();

        foreach (var schema in _schemas.EnumerateObject())
        {
            var name = schema.Name;
            if (name.EndsWith("Request") || name.EndsWith("Error") || name == "ApiKey")
                continue;
            if (name.EndsWith("Response") && !IsEntitySchema(name))
                continue;

            // Parse schemas that are referenced by multiple endpoints (entity models)
            if (IsEntitySchema(name))
            {
                sharedNames.Add(name);
            }
        }

        foreach (var name in sharedNames)
        {
            var model = ParseObjectSchema(name);
            if (model != null)
                result[name] = model;
        }

        return result;
    }

    private bool IsEntitySchema(string name)
    {
        // Entity schemas are ones that don't follow the {Group}{Action}Response pattern
        // or are referenced as array items
        var nonEntity = new[] { "Request", "Response" };
        if (name.EndsWith("Request")) return false;

        // Check if it's a simple object schema (not a Response wrapper)
        if (_schemas.TryGetProperty(name, out var schema))
        {
            if (schema.TryGetProperty("type", out var type))
            {
                var typeStr = type.ValueKind == JsonValueKind.String ? type.GetString() : null;
                if (typeStr == "array") return false;
            }
            if (schema.TryGetProperty("properties", out _)) return true;
            if (schema.TryGetProperty("allOf", out _)) return true;
            if (schema.TryGetProperty("$ref", out _)) return true;
        }
        return false;
    }

    private ApiGroup BuildGroup(string tag, List<ApiEndpoint> endpoints)
    {
        if (_nameMap.Groups.TryGetValue(tag, out var groupMap))
        {
            return new ApiGroup(tag, groupMap.Interface, groupMap.Class, groupMap.Property, endpoints);
        }

        var pascal = ToPascalCase(tag);
        return new ApiGroup(
            tag,
            $"IMandrill{pascal}Api",
            $"Mandrill{pascal}Api",
            pascal,
            endpoints
        );
    }

    private string GetMethodName(string operationId)
    {
        if (_nameMap.Operations.TryGetValue(operationId, out var opMap) && !string.IsNullOrEmpty(opMap.Method))
            return opMap.Method;

        // Default: take action part after slash, PascalCase it, add Async
        var parts = operationId.Split('/');
        var action = parts.Length > 1 ? parts[1] : parts[0];
        return ToPascalCase(action) + "Async";
    }

    private bool IsCustomImpl(string operationId)
    {
        if (_nameMap.Operations.TryGetValue(operationId, out var opMap))
            return opMap.CustomImpl;
        return false;
    }

    private string? GetRequestSchemaName(string operationId)
    {
        var pascal = OperationIdToSchemaName(operationId) + "Request";
        if (_schemas.TryGetProperty(pascal, out _))
            return pascal;

        // Try case-insensitive match
        foreach (var schema in _schemas.EnumerateObject())
        {
            if (schema.Name.Equals(pascal, StringComparison.OrdinalIgnoreCase) && schema.Name.EndsWith("Request"))
                return schema.Name;
        }
        return null;
    }

    private string GetResponseSchemaName(string operationId)
    {
        var pascal = OperationIdToSchemaName(operationId) + "Response";
        if (_schemas.TryGetProperty(pascal, out _))
            return pascal;

        // Try case-insensitive match
        foreach (var schema in _schemas.EnumerateObject())
        {
            if (schema.Name.Equals(pascal, StringComparison.OrdinalIgnoreCase) && schema.Name.EndsWith("Response"))
                return schema.Name;
        }
        return pascal;
    }

    private SchemaModel? ParseRequestSchema(string schemaName)
    {
        if (!_schemas.TryGetProperty(schemaName, out var schema))
            return null;

        var properties = new List<SchemaProperty>();

        if (schema.TryGetProperty("allOf", out var allOf))
        {
            // Collect required fields from ALL allOf items first
            var allRequired = new HashSet<string>();
            foreach (var item in allOf.EnumerateArray())
            {
                foreach (var r in GetRequiredFields(item))
                    allRequired.Add(r);
            }

            foreach (var item in allOf.EnumerateArray())
            {
                if (item.TryGetProperty("$ref", out var refEl))
                {
                    var refName = refEl.GetString()!.Split('/').Last();
                    if (refName == "ApiKey") continue; // Skip key, handled by MandrillRequestBase
                }
                if (item.TryGetProperty("properties", out var props))
                {
                    foreach (var prop in props.EnumerateObject())
                    {
                        if (prop.Name == "key") continue;
                        properties.Add(ParseProperty(prop.Name, prop.Value, allRequired.Contains(prop.Name)));
                    }
                }
            }
        }
        else if (schema.TryGetProperty("properties", out var directProps))
        {
            var required = GetRequiredFields(schema);
            foreach (var prop in directProps.EnumerateObject())
            {
                if (prop.Name == "key") continue;
                properties.Add(ParseProperty(prop.Name, prop.Value, required.Contains(prop.Name)));
            }
        }

        return new SchemaModel(schemaName, schema.TryGetProperty("description", out var d) ? d.GetString() : null, properties, false, null);
    }

    private SchemaModel? ParseResponseSchema(string schemaName)
    {
        if (!_schemas.TryGetProperty(schemaName, out var schema))
            return null;

        // Check for direct $ref
        if (schema.TryGetProperty("$ref", out var directRef))
        {
            var refName = directRef.GetString()!.Split('/').Last();
            return ParseResponseSchema(refName);
        }

        // Check for allOf with a single $ref (common pattern for response aliases)
        if (schema.TryGetProperty("allOf", out var allOf))
        {
            foreach (var item in allOf.EnumerateArray())
            {
                if (item.TryGetProperty("$ref", out var itemRef))
                {
                    var refName = itemRef.GetString()!.Split('/').Last();
                    return ParseResponseSchema(refName);
                }
            }
        }

        // Check for oneOf where one option is an array and another is null
        if (schema.TryGetProperty("oneOf", out var topOneOf))
        {
            JsonElement? arrayOption = null;
            bool hasNullOption = false;

            foreach (var option in topOneOf.EnumerateArray())
            {
                if (option.TryGetProperty("type", out var t) && t.GetString() == "array")
                    arrayOption = option;
                else if (!option.TryGetProperty("type", out _) && !option.TryGetProperty("properties", out _) && !option.TryGetProperty("$ref", out _))
                    hasNullOption = true;
            }

            if (arrayOption.HasValue)
            {
                string? itemRef = null;
                var itemProperties = new List<SchemaProperty>();

                if (arrayOption.Value.TryGetProperty("items", out var items))
                {
                    if (items.TryGetProperty("$ref", out var r))
                        itemRef = r.GetString();
                    else if (items.TryGetProperty("properties", out var itemProps))
                    {
                        var itemRequired = GetRequiredFields(items);
                        foreach (var prop in itemProps.EnumerateObject())
                            itemProperties.Add(ParseProperty(prop.Name, prop.Value, itemRequired.Contains(prop.Name)));
                    }
                }

                return new SchemaModel(schemaName, null, itemProperties, IsArray: true, ArrayItemRef: itemRef, IsNullable: hasNullOption);
            }
        }

        // Check for string/primitive type (like UsersPingResponse which is just "string")
        if (schema.TryGetProperty("type", out var type))
        {
            var typeStr = type.ValueKind == JsonValueKind.String ? type.GetString() : null;
            if (typeStr == "array")
            {
                string? itemRef = null;
                var itemProperties = new List<SchemaProperty>();
                if (schema.TryGetProperty("items", out var items))
                {
                    if (items.TryGetProperty("$ref", out var r))
                        itemRef = r.GetString();
                    else if (items.TryGetProperty("oneOf", out var oneOf))
                    {
                        foreach (var option in oneOf.EnumerateArray())
                        {
                            if (option.TryGetProperty("$ref", out var oRef))
                            {
                                itemRef = oRef.GetString();
                                break;
                            }
                        }
                    }
                    else if (items.TryGetProperty("properties", out var itemProps))
                    {
                        var itemRequired = GetRequiredFields(items);
                        foreach (var prop in itemProps.EnumerateObject())
                        {
                            itemProperties.Add(ParseProperty(prop.Name, prop.Value, itemRequired.Contains(prop.Name)));
                        }
                    }
                }
                return new SchemaModel(schemaName, null, itemProperties, true, itemRef);
            }
            if (typeStr == "string")
            {
                // Primitive response - treat as empty schema (will be handled by customImpl)
                return new SchemaModel(schemaName, null, new List<SchemaProperty>(), false, null);
            }
        }

        // Regular object
        return ParseObjectSchema(schemaName);
    }

    public SchemaModel? ParseObjectSchemaPublic(string schemaName) => ParseObjectSchema(schemaName);

    private SchemaModel? ParseObjectSchema(string schemaName)
    {
        if (!_schemas.TryGetProperty(schemaName, out var schema))
            return null;

        // Handle direct $ref
        if (schema.TryGetProperty("$ref", out var directRef))
        {
            var refName = directRef.GetString()!.Split('/').Last();
            return ParseObjectSchema(refName);
        }

        var properties = new List<SchemaProperty>();
        var required = GetRequiredFields(schema);

        if (schema.TryGetProperty("properties", out var props))
        {
            foreach (var prop in props.EnumerateObject())
            {
                properties.Add(ParseProperty(prop.Name, prop.Value, required.Contains(prop.Name)));
            }
        }

        if (schema.TryGetProperty("allOf", out var allOf))
        {
            foreach (var item in allOf.EnumerateArray())
            {
                if (item.TryGetProperty("properties", out var allOfProps))
                {
                    var itemRequired = GetRequiredFields(item);
                    foreach (var prop in allOfProps.EnumerateObject())
                    {
                        properties.Add(ParseProperty(prop.Name, prop.Value, itemRequired.Contains(prop.Name) || required.Contains(prop.Name)));
                    }
                }
            }
        }

        var desc = schema.TryGetProperty("description", out var d) ? d.GetString() : null;
        return new SchemaModel(schemaName, desc, properties, false, null);
    }

    private SchemaProperty ParseProperty(string jsonName, JsonElement prop, bool isRequired)
    {
        var description = prop.TryGetProperty("description", out var d) ? d.GetString() : null;
        var isNullable = false;
        var isArray = false;
        var isEnum = false;
        List<string>? enumValues = null;
        string? refType = null;
        InlineObjectSchema? inlineObject = null;
        string typeStr = "object";

        if (prop.TryGetProperty("$ref", out var refEl))
        {
            refType = refEl.GetString()!.Split('/').Last();
            var csharpName = ToPascalCase(jsonName);
            return new SchemaProperty(jsonName, csharpName, MapSchemaName(refType), isRequired, false, false, false, null, refType, description, null);
        }

        if (prop.TryGetProperty("type", out var typeEl))
        {
            if (typeEl.ValueKind == JsonValueKind.Array)
            {
                var types = typeEl.EnumerateArray().Select(t => t.GetString()!).ToList();
                isNullable = types.Contains("null");
                typeStr = types.First(t => t != "null");
            }
            else
            {
                typeStr = typeEl.GetString() ?? "object";
            }
        }

        if (typeStr == "array")
        {
            isArray = true;
            if (prop.TryGetProperty("items", out var items))
            {
                if (items.TryGetProperty("$ref", out var itemRef))
                {
                    refType = itemRef.GetString()!.Split('/').Last();
                }
                else if (items.TryGetProperty("type", out var itemType))
                {
                    var itemTypeStr = itemType.ValueKind == JsonValueKind.String ? itemType.GetString() : "string";
                    if (itemTypeStr == "object" && items.TryGetProperty("properties", out var itemProps))
                    {
                        // Check if this inline item maps to a known schema
                        var inlineName = ToPascalCase(jsonName) + "Item";
                        var mappedName = MapSchemaName(inlineName);
                        if (mappedName != "Mandrill" + inlineName)
                        {
                            // Known mapped type - use it as a ref instead of inline
                            refType = inlineName;
                        }
                        else
                        {
                            // Truly inline - create nested class definition
                            var inlineProps = new List<SchemaProperty>();
                            var itemRequired = GetRequiredFields(items);
                            foreach (var ip in itemProps.EnumerateObject())
                            {
                                inlineProps.Add(ParseProperty(ip.Name, ip.Value, itemRequired.Contains(ip.Name)));
                            }
                            inlineObject = new InlineObjectSchema(inlineName, inlineProps);
                        }
                    }
                    else
                    {
                        refType = itemTypeStr; // primitive array like string[]
                    }
                }
                else
                {
                    // Items with no type and no $ref - default to string
                    refType = "string";
                }
            }
            else
            {
                // Array with no items definition - default to string
                refType = "string";
            }
        }

        if (prop.TryGetProperty("enum", out var enumEl))
        {
            isEnum = true;
            enumValues = enumEl.EnumerateArray()
                .Where(e => e.ValueKind == JsonValueKind.String)
                .Select(e => e.GetString()!)
                .ToList();
        }

        if (typeStr == "object" && !isArray && prop.TryGetProperty("properties", out var nestedProps))
        {
            var inlineName = ToPascalCase(jsonName);
            var mappedName = MapSchemaName(inlineName);
            if (mappedName != "Mandrill" + inlineName)
            {
                // Known mapped type - use it directly
                var csharpNameMapped = ToPascalCase(jsonName);
                return new SchemaProperty(jsonName, csharpNameMapped, mappedName + (isNullable ? "?" : ""), isRequired, isNullable, false, false, null, null, description, null);
            }

            var inlineProps = new List<SchemaProperty>();
            var nestedRequired = GetRequiredFields(prop);
            foreach (var np in nestedProps.EnumerateObject())
            {
                inlineProps.Add(ParseProperty(np.Name, np.Value, nestedRequired.Contains(np.Name)));
            }
            inlineObject = new InlineObjectSchema(inlineName, inlineProps);
        }

        var csharpType = MapJsonType(typeStr, isNullable, isRequired, isArray, isEnum, refType, inlineObject);
        var csName = ToPascalCase(jsonName);

        return new SchemaProperty(jsonName, csName, csharpType, isRequired, isNullable, isArray, isEnum, enumValues, refType, description, inlineObject);
    }

    private string MapJsonType(string jsonType, bool isNullable, bool isRequired, bool isArray, bool isEnum, string? refType, InlineObjectSchema? inlineObject)
    {
        if (isArray)
        {
            if (refType != null)
            {
                var mapped = MapPrimitiveOrRef(refType);
                return $"List<{mapped}>";
            }
            if (inlineObject != null)
            {
                // Inline objects become nested classes - don't pass through schema mapper
                return $"List<{inlineObject.CSharpName}>";
            }
            return "List<string>";
        }

        if (isEnum)
            return "string" + (isNullable ? "?" : "");

        if (inlineObject != null)
        {
            // Inline objects become nested classes - don't pass through schema mapper
            return inlineObject.CSharpName + (isNullable ? "?" : "");
        }

        var baseType = jsonType switch
        {
            "string" => "string",
            "integer" => "int",
            "number" => "double",
            "boolean" => "bool",
            "object" => "object",
            _ => "object"
        };

        if (baseType is "int" or "double" or "bool")
        {
            if (isNullable || !isRequired)
                return baseType + "?";
            return baseType;
        }

        if (baseType == "string")
            return isNullable ? "string?" : "string";

        return baseType + (isNullable ? "?" : "");
    }

    private string MapPrimitiveOrRef(string refType)
    {
        return refType switch
        {
            "string" => "string",
            "integer" => "int",
            "number" => "double",
            "boolean" => "bool",
            _ => MapSchemaName(refType)
        };
    }

    public string MapSchemaName(string specName)
    {
        if (_nameMap.Schemas.TryGetValue(specName, out var mapped))
            return mapped;

        // Default: add Mandrill prefix
        return "Mandrill" + specName;
    }

    private HashSet<string> GetRequiredFields(JsonElement schema)
    {
        if (schema.TryGetProperty("required", out var req))
        {
            return req.EnumerateArray().Select(r => r.GetString()!).ToHashSet();
        }
        return new HashSet<string>();
    }

    private string OperationIdToSchemaName(string operationId)
    {
        // "messages/send-template" -> "MessagesSendTemplate"
        var parts = operationId.Split('/');
        return string.Join("", parts.Select(ToPascalCase));
    }

    public static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var parts = input.Split(new[] { '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(p => char.ToUpper(p[0]) + p[1..]));
    }
}
