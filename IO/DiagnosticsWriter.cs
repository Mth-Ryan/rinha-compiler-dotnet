using System.Text;
using Rinha.Diagnostics;

namespace Rinha.IO;

public static class DiagnosticWriter
{
    public static void Write(Diagnostic diagnostic)
    {
        var local = diagnostic.Location;
        var messageBuilder = new StringBuilder($"{local.Filename}({local.Start}): ");
        if (diagnostic.Kind == DiagnosticKind.Error)
        {
            messageBuilder.Append("error: ");
            messageBuilder.Append(diagnostic.Message);
            Console.Error.WriteLine(messageBuilder.ToString());
            return;
        }
        if (diagnostic.Kind == DiagnosticKind.Warnning)
        {
            messageBuilder.Append("warnning: ");
        }
        if (diagnostic.Kind == DiagnosticKind.Info)
        {
            messageBuilder.Append("info: ");
        }
        messageBuilder.Append(diagnostic.Message);
        Console.WriteLine(messageBuilder.ToString());
    }

    public static void WriteAll(IEnumerable<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
        {
            Write(diagnostic);
        }
    }
}
