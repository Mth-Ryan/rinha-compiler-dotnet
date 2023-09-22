using System.Collections.Immutable;
using Rinha.Diagnostics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;

namespace Rinha.Compilation.Emit;

public class Emitter
{
    public ImmutableArray<Diagnostic> EmitFile(string outputName, Node boundTree, string outputDir)
    {
        var assemblyName = new AssemblyNameDefinition($"{outputName}", new Version(1, 0, 0));
        var assembly = AssemblyDefinition.CreateAssembly(assemblyName, $"{outputName}", ModuleKind.Console);

        var module = assembly.MainModule;

        var objectRef = module.ImportReference(typeof(Object));
        var mainClassRef = new TypeDefinition("", "Program", TypeAttributes.Abstract | TypeAttributes.Sealed, objectRef);
        module.Types.Add(mainClassRef);

        var voidRef = module.ImportReference(typeof(void));
        var mainMethodRef = new MethodDefinition("Main", MethodAttributes.Static | MethodAttributes.Private, voidRef);
        mainClassRef.Methods.Add(mainMethodRef);

        var ilProcessor = mainMethodRef.Body.GetILProcessor();
        var writelineRef = module.ImportReference(typeof(Console).GetMethod("WriteLine", new [] { typeof(string) }));
        ilProcessor.Emit(OpCodes.Ldstr, "Hello world from Il");
        ilProcessor.Emit(OpCodes.Call, writelineRef);
        ilProcessor.Emit(OpCodes.Ret);

        assembly.EntryPoint = mainMethodRef;

        var targetDir = CreateEmitDirectory(outputName, outputDir);

        assembly.Write(Path.Combine(targetDir, $"{outputName}.dll"));
        EmitRuntimeConfig(outputName, targetDir);

        return new ImmutableArray<Diagnostic>();
    }

    private string CreateEmitDirectory(string filename, string baseDir)
    {
        var targetsDir = Path.Combine(baseDir, "targets");
        var targetSpecificDir = Path.Combine(targetsDir, filename);

        if (!Directory.Exists(targetsDir))
            Directory.CreateDirectory(targetsDir);

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
