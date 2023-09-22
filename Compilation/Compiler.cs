using System.Collections.Immutable;
using System.Text.Json;
using Rinha.Diagnostics;
using Rinha.Internal.AstJson;
using Rinha.Semantic;
using Rinha.Syntax;

namespace Rinha.Compilation;

public static class Compiler
{
    public static async Task<ImmutableArray<Diagnostic>> Compile(string filename, FileStream input, string outputPath, List<string> references)
    {
        return await CompileJson(filename, input, outputPath, references);
    }

    // FIXME: make the compilation real async
    private static async Task<ImmutableArray<Diagnostic>> CompileJson(string filename, FileStream input, string outputPath, List<string> references)
    {
        await Task.CompletedTask;
        var (ast, frontDiagnostics) = JsonFrontendPipeline(filename, input);
        if (frontDiagnostics.Length != 0)
        {
            return frontDiagnostics;
        }

        var backDiagnostics = BackendPipeline(filename, ast!, outputPath, references);
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

    private static ImmutableArray<Diagnostic> BackendPipeline(string filename, AstFile ast, string outputPath, List<string> references)
    {
        var binder = new Binder();
        var (bound, diagnostics) = binder.Bind(ast);

        var emmiter = new Emmit.Emmiter();
        emmiter.EmmitFile(filename, bound!, outputPath);

        return diagnostics;
    }
}
