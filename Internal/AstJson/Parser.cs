using System.Text.Json;
using Rinha.Syntax;

namespace Rinha.Internal.AstJson;

public class JsonParser
{
    private JsonSerializerOptions Options { get; init; }

    public JsonParser()
    {
        Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public AstFile? Parse(string input)
    {
        return JsonSerializer.Deserialize<AstFile>(input, Options);
    }

    public AstFile? Parse(FileStream input)
    {
        return JsonSerializer.Deserialize<AstFile>(input, Options);
    }
}

