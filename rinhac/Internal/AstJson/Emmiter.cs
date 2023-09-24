using System.Text.Json;
using Rinha.Syntax;

namespace Rinha.Internal.AstJson;

public class JsonEmmiter
{
    private JsonSerializerOptions Options { get; init; }

    public JsonEmmiter()
    {
        Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public string Emmit(AstFile input)
    {
        return JsonSerializer.Serialize(input, Options);
    }
}

