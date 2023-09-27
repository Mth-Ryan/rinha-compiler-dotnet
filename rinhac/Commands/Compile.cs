using System.Collections.Immutable;
using Rinha.Compilation;
using Rinha.Diagnostics;
using Rinha.IO;

namespace Rinha.Commands;

public static class Compile
{
    public static int Run(string outputPath, List<string> filePaths, List<string> references)
    {
        var diagnostics = CompileFiles(outputPath, filePaths, references);
        
        if (diagnostics.Length != 0)
        {
            DiagnosticWriter.WriteAll(diagnostics);
            return 1;
        }

        return 0;
    }

    private static ImmutableArray<Diagnostic> CompileFiles(
        string outputPath,
        List<string> filePaths,
        List<string> references)
    {
        var (sources, sourcesDiagnostics) = GetFileNameAndStream(filePaths);

        if (sourcesDiagnostics.Length != 0)
            return sourcesDiagnostics;

        var diagnostics = new List<ImmutableArray<Diagnostic>>();
        foreach (var source in sources)
        {
            diagnostics.Add(Compiler.Compile(
                source.Item1,
                source.Item2,
                outputPath,
                references));
        }

        return diagnostics.SelectMany(x => x).ToImmutableArray();
    }

    private static (List<(string, FileStream)>, ImmutableArray<Diagnostic>) GetFileNameAndStream(List<string> filePaths)
    {
        var diagnostics = new DiagnosticsBag();
        var sources = new List<(string, FileStream)>();

        foreach (var path in filePaths)
        {
            var filename = Path.GetFileNameWithoutExtension(path);

            try
            {
                var stream = File.Open(path, FileMode.Open);
                sources.Add((filename, stream));
            }
            catch (FileNotFoundException e)
            {
                _ = e;
                diagnostics.ReportFileNotFound(filename);
            }
        }

        return (sources, diagnostics.ToImmutableArray());
    }
}
