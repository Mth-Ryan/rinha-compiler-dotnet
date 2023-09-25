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

    public ImmutableArray<Diagnostic> EmitFile(Node boundTree, string outputDir)
    {

        var mainClassRef = EmitProgramClass();
        var mainMethodRef = EmitMainMethod(_module, mainClassRef);

        var ilProcessor = mainMethodRef.Body.GetILProcessor();

        _assembly.EntryPoint = mainMethodRef;

        var targetDir = CreateEmitDirectory(_moduleName, outputDir);

        EmitExpression(
            ilProcessor,
            new PrintExpr
            { 
                Value = new BinaryExpr
                {
                    Op = Syntax.BinaryOp.Rem,
                    Lhs = new IntegerExpr { Value = 5 },
                    Rhs = new IntegerExpr { Value = 2 }
                }
            });

        ilProcessor.Emit(OpCodes.Pop);
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
}
