using Mono.Cecil;
using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;

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

    private MethodDefinition EmitMainMethod(TypeDefinition mainClass)
    {
        return EmitMethod(
            mainClass,
            "Main",
            MethodAttributes.Static | MethodAttributes.Private,
            _knownTypes.GetRef(KnownType.SystemVoid));
    }

    private void EmitMethodBody(
        MethodDefinition method,
        Expression boundExpression,
        BoundScope scope,
        bool voidFunc = false)
    {
        var varSymbols = scope.GetAllBlockVariables();
        var locals = varSymbols.ToDictionary(
            s => s,
            s => new VariableDefinition(_knownTypes.GetRef(KnownType.RinhaObject)));

        foreach (var def in locals.Values)
        {
            method.Body.Variables.Add(def);
        }

        var il = method.Body.GetILProcessor();
        EmitExpression(il, locals, null, boundExpression);

        if (voidFunc)
            il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);
    }
}
