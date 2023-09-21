using System.Collections;
using Rinha.Syntax;

namespace Rinha.Diagnostics;

public class DiagnosticsBagEnumerator : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>(); 

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _diagnostics.GetEnumerator();

    public void AddRange(IEnumerable<Diagnostic> diagnostics)
    {
        _diagnostics.AddRange(diagnostics);
    }

    private void ReportError(string message, Location location)
    {
        var diagnostic = Diagnostic.Error(message, location);
        _diagnostics.Add(diagnostic);
    }

    private void ReportWarn(string message, Location location)
    {
        var diagnostic = Diagnostic.Warnning(message, location);
        _diagnostics.Add(diagnostic);
    }

    private void ReportInfo(string message, Location location)
    {
        var diagnostic = Diagnostic.Info(message, location);
        _diagnostics.Add(diagnostic);
    }
}

