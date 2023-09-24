using Mono.Cecil;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    private TypeDefinition EmitClass(
        string name,
        TypeAttributes attributes,
        TypeReference baseType)
    {

        var classRef = new TypeDefinition("", name, attributes, baseType);
        _module.Types.Add(classRef);

        return classRef;
    }

    private TypeDefinition EmitProgramClass()
    {
        return EmitClass(
            "Program",
            TypeAttributes.Abstract | TypeAttributes.Sealed,
            _knownTypes[typeof(object)]);
    }
}
