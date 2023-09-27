using Mono.Cecil;
using Mono.Cecil.Cil;
using Rinha.Semantic;
using Rinha.Semantic.BoundTree;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    private TypeDefinition EmitClosureClass(FunctionSymbol function)
    {
        var name = $"Closure_{function.GetHashCode().ToString("X")}";
        return EmitClass(name,
            TypeAttributes.Sealed,
            _knownTypes.GetRef(KnownType.RinhaInnerClosure));
    }

    private void EmitClosureCtor(TypeDefinition closureClass)
    {
        var method = EmitMethod(
            closureClass,
            ".ctor",
            MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName,
            _knownTypes.GetRef(KnownType.SystemVoid));

        var il = method.Body.GetILProcessor();

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, _knownMethods.GetRef(KnownMethod.RinhaInnerClosureCtor));
        il.Emit(OpCodes.Ret);
    }

    private (MethodDefinition, ParameterDefinition) EmitClosureMethod(TypeDefinition closureClass)
    {
        var method = EmitMethod(
            closureClass,
            "Run",
            MethodAttributes.Public |
            MethodAttributes.Virtual |
            MethodAttributes.HideBySig,
            _knownTypes.GetRef(KnownType.RinhaObject));

        var args = new ParameterDefinition(
            "args",
            ParameterAttributes.None,
            _knownTypes.GetRef(KnownType.RinhaObjectArr));

        method.Parameters.Add(args);

        return (method, args);
    }

    private void EmitClosureBody(
        MethodDefinition method,
        ParameterDefinition param,
        List<VariableSymbol> paramsSymbols,
        Expression boundExpression,
        BoundScope scope)
    {
        var varSymbols = scope.GetAllBlockVariables();
        var locals = varSymbols.ToDictionary(
            s => s,
            s => new VariableDefinition(_knownTypes.GetRef(KnownType.RinhaObject)));

        var args = paramsSymbols
            .Select((s, index) => (s, index))
            .ToDictionary(
                t => t.s,
                t => (param, t.index)
            );

        foreach (var def in locals.Values)
        {
            method.Body.Variables.Add(def);
        }

        var il = method.Body.GetILProcessor();
        EmitExpression(il, locals, args, boundExpression);

        il.Emit(OpCodes.Ret);
    }

    private TypeDefinition EmitClosure(LambdaExpr function)
    {
        var closureClass = EmitClosureClass(function.Symbol);
        EmitClosureCtor(closureClass);
        var (method, param) = EmitClosureMethod(closureClass);
        EmitClosureBody(
            method,
            param,
            function.ParametersSymbols,
            function.Body,
            function.Scope!);

        return closureClass;
    }

    private Dictionary<FunctionSymbol, TypeDefinition> EmitAllClosuresDefinitions(BoundProgram program)
    {
        var table = program.Functions.ToDictionary(
            f => f.Symbol,
            f => EmitClosure(f)
        );

        return table;
    }

    private MethodDefinition? FindClosureCtor(FunctionSymbol function)
    {
        var closure = _closures[function];
        return closure.Methods.FirstOrDefault(c => c.IsConstructor);
    }
}
