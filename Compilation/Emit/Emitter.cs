using System.Collections.Immutable;
using Rinha.Diagnostics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    private string _moduleName;
    private AssemblyDefinition _assembly;
    private ModuleDefinition _module;
    private Dictionary<Type, TypeReference> _knownTypes;

    public Emitter(string filename)
    {
        _moduleName = filename;

        var assemblyName = new AssemblyNameDefinition(_moduleName, new Version(1, 0, 0));
        _assembly = AssemblyDefinition
            .CreateAssembly(assemblyName, _moduleName, ModuleKind.Console);

        _module = _assembly.MainModule;

        _knownTypes = GetKnownTypes(_module);
    }

    public ImmutableArray<Diagnostic> EmitFile(Node boundTree, string outputDir)
    {

        var mainClassRef = EmitProgramClass();
        var mainMethodRef = EmitMainMethod(_module, mainClassRef);

        var ilProcessor = mainMethodRef.Body.GetILProcessor();

        _assembly.EntryPoint = mainMethodRef;

        var targetDir = CreateEmitDirectory(_moduleName, outputDir);
        EmitExpression(ilProcessor, new PrintExpr { Value = new StringExpr { Value = "Hello from emmiter" } });
        ilProcessor.Emit(OpCodes.Ret);

        _assembly.Write(Path.Combine(targetDir, $"{_moduleName}.dll"));
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

    private Dictionary<Type, TypeReference> GetKnownTypes(ModuleDefinition module)
    {
        var knownTypes = new Dictionary<Type, TypeReference>();

        knownTypes[typeof(object)] = module.ImportReference(typeof(object));
        knownTypes[typeof(int)] = module.ImportReference(typeof(int));
        knownTypes[typeof(string)] = module.ImportReference(typeof(string));
        knownTypes[typeof(Console)] = module.ImportReference(typeof(Console));

        return knownTypes;
    }
}
