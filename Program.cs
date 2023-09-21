using Mono.Options;
using Rinha.Commands;

var referencePaths = new List<string>();
var sourcePaths = new List<string>();
var helpRequest = false;

var options = new OptionSet
{
    "usage: rinhac <source-paths...> [options]",
    { "r=", "The {path} of an assembly to reference", v => referencePaths.Add(v) },
    { "h|help", "Prints help", v => helpRequest = true },
    { "<>", v => sourcePaths.Add(v) },
};

options.Parse(args);

if (helpRequest)
{
    Help.Run(options);
    return;
}

if (args.Length == 0)
{
    Console.Error.WriteLine("need at least one source file");
    Environment.ExitCode = 1;
    return;
}

var exitCode = await Compile.Run(sourcePaths, referencePaths);
Environment.ExitCode = exitCode;
