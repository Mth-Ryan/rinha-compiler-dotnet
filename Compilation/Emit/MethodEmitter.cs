using Mono.Cecil;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    private MethodDefinition EmitMethod(
        TypeDefinition classRef,
        string name,
        MethodAttributes attributes,
        TypeReference returnType)
    {

        var methodRef = new MethodDefinition(name, attributes, returnType);
        classRef.Methods.Add(methodRef);
        return methodRef;
    }

    private MethodDefinition EmitMainMethod(ModuleDefinition module, TypeDefinition mainClass)
    {
        var voidRef = module.ImportReference(typeof(void));
        return EmitMethod(
            mainClass,
            "Main",
            MethodAttributes.Static | MethodAttributes.Private,
            voidRef);
    }
}