using System.Collections.Immutable;
using System.Text.Json;
using Rinha.Diagnostics;
using Rinha.Internal.AstJson;

namespace Rinha.Compilation;

public class Compiler
{
    private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();

    public Task<ImmutableArray<Diagnostic>> Compile(string filename, FileStream input, List<string> references)
    {
        return CompileJson(filename, input, references);
    }

    private async Task<ImmutableArray<Diagnostic>> CompileJson(string filename, FileStream input, List<string> references)
    {
        var jsonParser = new JsonParser();
        var jsonEmmiter = new JsonEmmiter();

        try
        {
            var ast = jsonParser.Parse(input);
            var json = jsonEmmiter.Emmit(ast!);

            Console.WriteLine(json);
        }
        catch (ArgumentNullException e)
        {
            _ = e;
            _diagnostics.ReportNullFile(filename);
        }
        catch (JsonException e)
        {
            _ = e;
            _diagnostics.ReportInvalidFileFormat(filename);
        }
        catch (NotSupportedException e)
        {
            _ = e;
            _diagnostics.ReportInvalidFileFormat(filename);
        }

        return _diagnostics.ToImmutableArray();
    }
}
