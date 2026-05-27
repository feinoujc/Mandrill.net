namespace Mandrill.CodeGen;

public record ApiModel(List<ApiGroup> Groups, Dictionary<string, SchemaModel> SharedSchemas);

public record ApiGroup(string Tag, string InterfaceName, string ClassName, string PropertyName, List<ApiEndpoint> Endpoints);

public record ApiEndpoint(
    string OperationId,
    string Path,
    string MethodName,
    string RequestTypeName,
    SchemaModel? RequestSchema,
    string ResponseTypeName,
    SchemaModel? ResponseSchema,
    bool IsArrayResponse,
    string? ArrayItemTypeName,
    bool CustomImpl
);

public record SchemaModel(
    string Name,
    string? Description,
    List<SchemaProperty> Properties,
    bool IsArray,
    string? ArrayItemRef,
    bool IsNullable = false
);

public record SchemaProperty(
    string JsonName,
    string CSharpName,
    string CSharpType,
    bool IsRequired,
    bool IsNullable,
    bool IsArray,
    bool IsEnum,
    List<string>? EnumValues,
    string? RefType,
    string? Description,
    InlineObjectSchema? InlineObject
);

public record InlineObjectSchema(string CSharpName, List<SchemaProperty> Properties);
