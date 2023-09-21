using Mono.Options;

namespace Rinha.Commands;

public static class Help
{
    public static void Run(OptionSet options)
    {
        options.WriteOptionDescriptions(Console.Out);
    }
}
