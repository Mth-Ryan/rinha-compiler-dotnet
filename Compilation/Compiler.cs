using System.Collections.Immutable;
using System.Text.Json;
using Rinha.Diagnostics;
using Rinha.Internal.AstJson;
using Rinha.Syntax;

namespace Rinha.Compilation;

public static class Compiler
{
    public static async Task<ImmutableArray<Diagnostic>> Compile(string filename, FileStream input, List<string> references)
    {
        return await CompileJson(filename, input, references);
    }

    // FIXME: make the compilation real async
    private static async Task<ImmutableArray<Diagnostic>> CompileJson(string filename, FileStream input, List<string> references)
    {
        var (ast, frontDiagnostics) = JsonFrontendPipeline(filename, input);
        if (frontDiagnostics.Length != 0)
        {
            return frontDiagnostics;
        }

        var backDiagnostics = BackendPipeline(ast!, references);
        return backDiagnostics;
    }

    private static (AstFile?, ImmutableArray<Diagnostic>) JsonFrontendPipeline(string filename, FileStream input)
    {
        var diagnostics = new DiagnosticsBag();
        var jsonParser = new JsonParser();

        try
        {
            var ast = jsonParser.Parse(input);
            return (ast, diagnostics.ToImmutableArray());
        }
        catch (ArgumentNullException e)
        {
            _ = e;
            diagnostics.ReportNullFile(filename);
        }
        catch (JsonException e)
        {
            _ = e;
            diagnostics.ReportInvalidFileFormat(filename);
        }
        catch (NotSupportedException e)
        {
            _ = e;
            diagnostics.ReportInvalidFileFormat(filename);
        }

        return (null, diagnostics.ToImmutableArray());
    }

    private static ImmutableArray<Diagnostic> BackendPipeline(AstFile ast, List<string> references)
    {
        var jsonEmmiter = new JsonEmmiter();
        Console.WriteLine(jsonEmmiter.Emmit(ast));

        return new DiagnosticsBag().ToImmutableArray();
    }
}
