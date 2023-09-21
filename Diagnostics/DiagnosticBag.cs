using System.Collections;
using Rinha.Syntax;

namespace Rinha.Diagnostics;

public class DiagnosticsBag : IEnumerable<Diagnostic>
{
    public DiagnosticsBag() {}

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

    public void ReportNullFile(string filename)
    {
        var location = new Location
        {
            Start = 0,
            End = 0,
            Filename = filename
        };
        ReportError("Null file input", location);
    }

    public void ReportInvalidFileFormat(string filename)
    {
        var location = new Location
        {
            Start = 0,
            End = 0,
            Filename = filename
        };
        ReportError("Invalid file format", location);
    }


    public void ReportFileNotFound(string filename)
    {
        var location = new Location
        {
            Start = 0,
            End = 0,
            Filename = filename
        };
        ReportError("File not found", location);
    }
}

