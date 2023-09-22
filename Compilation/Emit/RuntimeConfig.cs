using System.Text.Json;

namespace Rinha.Compilation.Emit;



public class RuntimeConfig
{
    public RuntimeConfig()
    {
        var netVer = System.Environment.Version;

        RuntimeOptions = new Options
        {
            Tfm = $"net{netVer.Major}.{netVer.Minor}",
            Framework = new Framework
            {
                Name = "Microsoft.NETCore.App",
                Version = $"{netVer.Major}.{netVer.Minor}.0"
            }
        };
    }

    public class Framework
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
    }

    public class Options
    {
        public required string Tfm { get; set; }
        public required Framework Framework { get; set; }
    }

    public Options RuntimeOptions { get; set; }

    public string? ToJson()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        return JsonSerializer.Serialize(this, options);
    }
}
