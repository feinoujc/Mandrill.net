using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mandrill.CodeGen;

public class NameMap
{
    [JsonPropertyName("groups")]
    public Dictionary<string, GroupMap> Groups { get; set; } = new();

    [JsonPropertyName("operations")]
    public Dictionary<string, OperationMap> Operations { get; set; } = new();

    [JsonPropertyName("schemas")]
    public Dictionary<string, string> Schemas { get; set; } = new();

    [JsonPropertyName("exclude")]
    public List<string> Exclude { get; set; } = new();

    public static NameMap Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<NameMap>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        })!;
    }
}

public class GroupMap
{
    [JsonPropertyName("interface")]
    public string Interface { get; set; } = "";

    [JsonPropertyName("class")]
    public string Class { get; set; } = "";

    [JsonPropertyName("property")]
    public string Property { get; set; } = "";
}

public class OperationMap
{
    [JsonPropertyName("method")]
    public string Method { get; set; } = "";

    [JsonPropertyName("customImpl")]
    public bool CustomImpl { get; set; }
}
