using System.Collections.Immutable;
using Rinha.Diagnostics;
using Mono.Cecil;
using Rinha.Semantic;
using System.Reflection;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    private string _moduleName;
    private AssemblyDefinition _assembly;
    private ModuleDefinition _module;
    private KnownTypes _knownTypes;
    private KnownMethods _knownMethods;

    public Emitter(string filename)
    {
        _moduleName = filename;

        var assemblyName = new AssemblyNameDefinition(_moduleName, new Version(1, 0, 0));
        _assembly = AssemblyDefinition
            .CreateAssembly(assemblyName, _moduleName, ModuleKind.Console);

        _module = _assembly.MainModule;

        _knownTypes = new KnownTypes(_module);
        _knownMethods = new KnownMethods(_module);
    }

    public ImmutableArray<Diagnostic> EmitFile(BoundProgram program, string outputDir)
    {

        var mainClassRef = EmitProgramClass();
        var mainMethodRef = EmitMainMethod(mainClassRef);

        EmitMethodBody(mainMethodRef, program.BoundTree, program.GlobalScope, true);

        _assembly.EntryPoint = mainMethodRef;
        var targetDir = CreateEmitDirectory(_moduleName, outputDir);
        _assembly.Write(Path.Combine(targetDir, $"{_moduleName}.dll"));
        EmitCoreLib(targetDir);
        EmitRuntimeConfig(_moduleName, targetDir);

        return new ImmutableArray<Diagnostic>();
    }


    private string CreateEmitDirectory(string filename, string baseDir)
    {
        var targetSpecificDir = Path.Combine(baseDir, filename);

        if (!Directory.Exists(targetSpecificDir))
            Directory.CreateDirectory(targetSpecificDir);

        return targetSpecificDir;
    }

    private void EmitCoreLib(string targetDir)
    {
        var selfTargetDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var sourceCore = Path.Combine(selfTargetDir!, "Rinha.Core.dll");
        var targetFile = Path.Combine(targetDir, "Rinha.Core.dll");
        
        if (File.Exists(targetFile))
        {
            File.Delete(targetFile);
        }
        File.Copy(sourceCore, targetFile);
    }

    private void EmitRuntimeConfig(string filename, string targetDir)
    {
        var config = new RuntimeConfig();
        var configName = $"{filename}.runtimeconfig.json";
        var path = Path.Combine(targetDir, configName);

        if (File.Exists(path))
            File.Delete(path);

        using (StreamWriter outputFile = new StreamWriter(path, true))
        {
            outputFile.Write(config.ToJson()!);
        }
    }
}
