using Scriban;
using Scriban.Runtime;

namespace Mandrill.CodeGen;

public class Generator
{
    private readonly ApiModel _model;
    private readonly NameMap _nameMap;
    private readonly string _outputDir;
    private readonly string _templateDir;
    private readonly OpenApiParser _parser;

    public Generator(ApiModel model, NameMap nameMap, string outputDir, string templateDir, OpenApiParser parser)
    {
        _model = model;
        _nameMap = nameMap;
        _outputDir = outputDir;
        _templateDir = templateDir;
        _parser = parser;
    }

    public void Generate()
    {
        if (Directory.Exists(_outputDir))
            Directory.Delete(_outputDir, true);
        Directory.CreateDirectory(_outputDir);

        GenerateTopLevelInterface();
        GenerateTopLevelClass();

        foreach (var group in _model.Groups)
        {
            var groupDir = Path.Combine(_outputDir, group.PropertyName);
            Directory.CreateDirectory(groupDir);

            GenerateRequestModels(group, groupDir);
            GenerateResponseModels(group, groupDir);
            GenerateSubApiInterface(group, groupDir);
            GenerateSubApiImplementation(group, groupDir);
        }
    }

    private void GenerateTopLevelInterface()
    {
        var template = LoadTemplate("TopLevelInterface.scriban");
        var props = _model.Groups.Select(g => new
        {
            interface_type = g.InterfaceName,
            name = g.PropertyName
        }).ToList();

        var result = template.Render(new { properties = props }, member => member.Name);
        File.WriteAllText(Path.Combine(_outputDir, "IMandrillApi.g.cs"), result);
    }

    private void GenerateTopLevelClass()
    {
        var template = LoadTemplate("TopLevelClass.scriban");
        var props = _model.Groups.Select(g => new
        {
            interface_name = g.InterfaceName,
            class_name = g.ClassName,
            property_name = g.PropertyName,
            field_name = char.ToLower(g.PropertyName[0]) + g.PropertyName[1..]
        }).ToList();

        var result = template.Render(new { properties = props }, member => member.Name);
        File.WriteAllText(Path.Combine(_outputDir, "MandrillApi.g.cs"), result);
    }

    private void GenerateRequestModels(ApiGroup group, string groupDir)
    {
        var classes = new List<object>();
        var seenTypes = new HashSet<string>();

        foreach (var endpoint in group.Endpoints)
        {
            if (endpoint.RequestSchema == null) continue;
            if (endpoint.CustomImpl) continue;
            if (_nameMap.Exclude.Contains(endpoint.RequestTypeName)) continue;
            if (!seenTypes.Add(endpoint.RequestTypeName)) continue;

            var props = endpoint.RequestSchema.Properties.Select(p => new
            {
                type = p.CSharpType,
                csharp_name = p.CSharpName,
                json_property_name = NeedsJsonPropertyName(p) ? p.JsonName : null,
                default_value = GetDefaultValue(p)
            }).ToList();

            classes.Add(new
            {
                name = endpoint.RequestTypeName,
                properties = props
            });
        }

        if (classes.Count == 0) return;

        var template = LoadTemplate("RequestModel.scriban");
        var result = template.Render(new { classes }, member => member.Name);
        File.WriteAllText(Path.Combine(groupDir, $"{group.PropertyName}Requests.g.cs"), result);
    }

    private void GenerateResponseModels(ApiGroup group, string groupDir)
    {
        var classes = new List<object>();
        var seenTypes = new HashSet<string>();

        foreach (var endpoint in group.Endpoints)
        {
            var schema = endpoint.ResponseSchema;
            if (schema == null) continue;
            if (schema.IsArray) continue; // Array responses use the item type, not a wrapper

            var typeName = endpoint.ResponseTypeName;
            if (_nameMap.Exclude.Contains(typeName)) continue;
            if (!seenTypes.Add(typeName)) continue;
            if (schema.Properties.Count == 0) continue; // Skip empty schemas ($ref wrappers)

            classes.Add(BuildResponseClass(typeName, schema.Properties));
        }

        // Also generate array item types from this group's endpoints
        foreach (var endpoint in group.Endpoints)
        {
            if (!endpoint.IsArrayResponse || endpoint.ArrayItemTypeName == null) continue;
            if (_nameMap.Exclude.Contains(endpoint.ArrayItemTypeName)) continue;
            if (!seenTypes.Add(endpoint.ArrayItemTypeName)) continue;

            SchemaModel? itemSchema = null;
            if (endpoint.ResponseSchema?.ArrayItemRef != null)
            {
                var itemSchemaName = endpoint.ResponseSchema.ArrayItemRef.Split('/').Last();
                itemSchema = ParseSharedSchema(itemSchemaName);
            }
            else if (endpoint.ResponseSchema?.Properties.Count > 0)
            {
                itemSchema = endpoint.ResponseSchema;
            }

            if (itemSchema == null || itemSchema.Properties.Count == 0) continue;

            classes.Add(BuildResponseClass(endpoint.ArrayItemTypeName, itemSchema.Properties));
        }

        // Also generate shared schemas that belong to this group
        foreach (var kvp in _model.SharedSchemas)
        {
            var schemaName = kvp.Key;
            var schema = kvp.Value;
            var mappedName = _nameMap.Schemas.GetValueOrDefault(schemaName, "Mandrill" + schemaName);

            if (_nameMap.Exclude.Contains(mappedName)) continue;
            if (!seenTypes.Add(mappedName)) continue;

            // Check if this schema belongs to this group
            if (!schemaName.StartsWith(OpenApiParser.ToPascalCase(group.Tag), StringComparison.OrdinalIgnoreCase))
                continue;

            classes.Add(BuildResponseClass(mappedName, schema.Properties));
        }

        if (classes.Count == 0) return;

        var template = LoadTemplate("ResponseModel.scriban");
        var result = template.Render(new { classes }, member => member.Name);
        File.WriteAllText(Path.Combine(groupDir, $"{group.PropertyName}Models.g.cs"), result);
    }

    private void GenerateSubApiInterface(ApiGroup group, string groupDir)
    {
        var methods = group.Endpoints.Select(e =>
        {
            var returnType = e.IsArrayResponse
                ? (e.ResponseSchema?.IsNullable == true ? $"IList<{e.ArrayItemTypeName}>?" : $"IList<{e.ArrayItemTypeName}>")
                : e.ResponseTypeName;

            var parameters = BuildMethodParameters(e);

            return new
            {
                name = e.MethodName,
                return_type = returnType,
                parameters = parameters.Length > 0 ? parameters + ", " : "",
                custom_impl = e.CustomImpl
            };
        }).ToList();

        var template = LoadTemplate("SubApiInterface.scriban");
        var result = template.Render(new
        {
            interfaces = new[]
            {
                new
                {
                    name = group.InterfaceName,
                    methods
                }
            }
        }, member => member.Name);

        File.WriteAllText(Path.Combine(groupDir, $"{group.InterfaceName}.g.cs"), result);
    }

    private void GenerateSubApiImplementation(ApiGroup group, string groupDir)
    {
        var methods = group.Endpoints.Select(e =>
        {
            var returnType = e.IsArrayResponse
                ? (e.ResponseSchema?.IsNullable == true ? $"IList<{e.ArrayItemTypeName}>?" : $"IList<{e.ArrayItemTypeName}>")
                : e.ResponseTypeName;

            var parameters = BuildMethodParameters(e);
            var assignments = BuildAssignments(e);
            var url = e.Path + ".json";

            return new
            {
                name = e.MethodName,
                return_type = returnType,
                parameters = parameters.Length > 0 ? parameters + ", " : "",
                request_type = e.RequestTypeName,
                url,
                assignments,
                custom_impl = e.CustomImpl
            };
        }).ToList();

        var template = LoadTemplate("SubApiImplementation.scriban");
        var result = template.Render(new
        {
            implementations = new[]
            {
                new
                {
                    class_name = group.ClassName,
                    interface_name = group.InterfaceName,
                    methods
                }
            }
        }, member => member.Name);

        File.WriteAllText(Path.Combine(groupDir, $"{group.ClassName}.g.cs"), result);
    }

    private string BuildMethodParameters(ApiEndpoint endpoint)
    {
        if (endpoint.RequestSchema == null) return "";

        var props = endpoint.RequestSchema.Properties;
        var parts = new List<string>();

        // Required params first, then optional
        var required = props.Where(p => p.IsRequired).ToList();
        var optional = props.Where(p => !p.IsRequired).ToList();

        foreach (var prop in required)
        {
            var paramName = CamelCase(prop.CSharpName);
            var type = prop.CSharpType;
            parts.Add($"{type} {paramName}");
        }

        foreach (var prop in optional)
        {
            var paramName = CamelCase(prop.CSharpName);
            var type = prop.CSharpType;

            if (!type.EndsWith("?"))
            {
                type += "?";
            }

            parts.Add($"{type} {paramName} = default");
        }

        return string.Join(", ", parts);
    }

    private List<object> BuildAssignments(ApiEndpoint endpoint)
    {
        if (endpoint.RequestSchema == null) return new List<object>();

        return endpoint.RequestSchema.Properties.Select(p => (object)new
        {
            property = p.CSharpName,
            value = CamelCase(p.CSharpName)
        }).ToList();
    }

    private SchemaModel? ParseSharedSchema(string schemaName)
    {
        if (_model.SharedSchemas.TryGetValue(schemaName, out var cached))
            return cached;
        return _parser.ParseObjectSchemaPublic(schemaName);
    }

    private object BuildResponseClass(string typeName, List<SchemaProperty> schemaProperties)
    {
        var nestedClasses = new List<object>();
        var propertyNames = schemaProperties.Select(p => p.CSharpName).ToHashSet();

        var props = schemaProperties.Select(p =>
        {
            var propType = p.CSharpType;

            if (p.InlineObject != null && !_nameMap.Exclude.Contains(p.InlineObject.CSharpName))
            {
                var nestedName = p.InlineObject.CSharpName;

                // Resolve collision: nested class name == property name
                if (propertyNames.Contains(nestedName))
                {
                    nestedName += "Data";
                    propType = propType.Replace(p.InlineObject.CSharpName, nestedName);
                }

                nestedClasses.Add(new
                {
                    name = nestedName,
                    properties = p.InlineObject.Properties.Select(np => new
                    {
                        type = np.CSharpType,
                        csharp_name = np.CSharpName,
                        json_property_name = NeedsJsonPropertyName(np) ? np.JsonName : null,
                        default_value = GetDefaultValue(np)
                    }).ToList()
                });
            }

            return new
            {
                type = propType,
                csharp_name = p.CSharpName,
                json_property_name = NeedsJsonPropertyName(p) ? p.JsonName : null,
                default_value = GetDefaultValue(p)
            };
        }).ToList();

        return new
        {
            name = typeName,
            properties = props,
            nested_classes = nestedClasses
        };
    }

    private bool NeedsJsonPropertyName(SchemaProperty prop)
    {
        var expected = ToSnakeCase(prop.CSharpName);
        return prop.JsonName != expected || prop.JsonName.StartsWith("_");
    }

    private string? GetDefaultValue(SchemaProperty prop)
    {
        if (prop.IsArray || prop.CSharpType.StartsWith("List<"))
            return $"new()";
        return null;
    }

    private static string CamelCase(string pascalCase)
    {
        if (string.IsNullOrEmpty(pascalCase)) return pascalCase;
        if (pascalCase.Length == 1) return pascalCase.ToLower();

        // Handle multi-char abbreviations like "IP" -> "ip"
        if (pascalCase.Length >= 2 && char.IsUpper(pascalCase[0]) && char.IsUpper(pascalCase[1]))
        {
            var i = 0;
            while (i < pascalCase.Length && char.IsUpper(pascalCase[i])) i++;
            if (i == pascalCase.Length) return pascalCase.ToLower();
            return pascalCase[..(i - 1)].ToLower() + pascalCase[(i - 1)..];
        }

        return char.ToLower(pascalCase[0]) + pascalCase[1..];
    }

    private static string ToSnakeCase(string pascalCase)
    {
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < pascalCase.Length; i++)
        {
            if (char.IsUpper(pascalCase[i]))
            {
                if (i > 0) result.Append('_');
                result.Append(char.ToLower(pascalCase[i]));
            }
            else
            {
                result.Append(pascalCase[i]);
            }
        }
        return result.ToString();
    }

    private Template LoadTemplate(string name)
    {
        var path = Path.Combine(_templateDir, name);
        var text = File.ReadAllText(path);
        return Template.Parse(text, path);
    }
}
