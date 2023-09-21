using System.Collections.Immutable;
using Rinha.Compilation;
using Rinha.Diagnostics;
using Rinha.IO;

namespace Rinha.Commands;

public static class Compile
{
    public static async Task<int> Run(List<string> filePaths, List<string> references)
    {
        var diagnostics = await CompileFiles(filePaths, references);
        
        if (diagnostics.Length != 0)
        {
            DiagnosticWriter.WriteAll(diagnostics);
            return 1;
        }

        return 0;
    }

    private static async Task<ImmutableArray<Diagnostic>> CompileFiles(List<string> filePaths, List<string> references)
    {
        var compiler = new Compiler();

        var (sources, sourcesDiagnostics) = GetFileNameAndStream(filePaths);

        if (sourcesDiagnostics.Length != 0)
            return sourcesDiagnostics;

        var tasks = new List<Task<ImmutableArray<Diagnostic>>>();
        foreach (var source in sources)
        {
            tasks.Append(compiler.Compile(source.Item1, source.Item2, references));
        }

        var diagosticsArr = await Task.WhenAll(tasks);
        return diagosticsArr.SelectMany(x => x).ToImmutableArray();
    }

    private static (List<(string, FileStream)>, ImmutableArray<Diagnostic>) GetFileNameAndStream(List<string> filePaths)
    {
        var diagnostics = new DiagnosticsBag();
        var sources = new List<(string, FileStream)>();

        foreach (var path in filePaths)
        {
            var filename = Path.GetFileName(path);

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
