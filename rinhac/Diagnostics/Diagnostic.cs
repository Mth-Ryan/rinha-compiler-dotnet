using Rinha.Syntax;

namespace Rinha.Diagnostics;

public struct Diagnostic 
{
    public DiagnosticKind Kind { get; init; }
    public string Message { get; init; }
    public Location Location { get; init; }

    public static Diagnostic Error(string message, Location location)
    {
        return new Diagnostic
        {
            Kind = DiagnosticKind.Error,
            Message = message,
            Location = location
        };
    }

    public static Diagnostic Warnning(string message, Location location)
    {
        return new Diagnostic
        {
            Kind = DiagnosticKind.Warnning,
            Message = message,
            Location = location
        };
    }

    public static Diagnostic Info(string message, Location location)
    {
        return new Diagnostic
        {
            Kind = DiagnosticKind.Info,
            Message = message,
            Location = location
        };
    }
}
