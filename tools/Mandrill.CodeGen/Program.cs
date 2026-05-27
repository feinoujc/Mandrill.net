using Mandrill.CodeGen;

var repoRoot = FindRepoRoot(Environment.CurrentDirectory);
var specPath = Path.Combine(repoRoot, "spec", "mandrill-openapi.json");
var nameMapPath = Path.Combine(repoRoot, "tools", "Mandrill.CodeGen", "name-map.json");
var outputDir = Path.Combine(repoRoot, "src", "Mandrill.net", "Generated");
var templateDir = Path.Combine(repoRoot, "tools", "Mandrill.CodeGen", "Templates");

Console.WriteLine($"Spec: {specPath}");
Console.WriteLine($"Output: {outputDir}");

var nameMap = NameMap.Load(nameMapPath);
var parser = new OpenApiParser(specPath, nameMap);
var model = parser.Parse();

Console.WriteLine($"Parsed {model.Groups.Count} groups, {model.Groups.Sum(g => g.Endpoints.Count)} endpoints");

var generator = new Generator(model, nameMap, outputDir, templateDir, parser);
generator.Generate();

Console.WriteLine("Code generation complete.");

static string FindRepoRoot(string startDir)
{
    var dir = startDir;
    while (dir != null)
    {
        if (Directory.Exists(Path.Combine(dir, ".git")))
            return dir;
        dir = Path.GetDirectoryName(dir);
    }
    throw new InvalidOperationException("Could not find repository root");
}
