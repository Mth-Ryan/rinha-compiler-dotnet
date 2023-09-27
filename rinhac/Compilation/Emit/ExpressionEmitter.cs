using Mono.Cecil.Cil;
using Mono.Cecil;
using Rinha.Semantic.BoundTree;
using Rinha.Syntax;
using System.Collections.Immutable;

namespace Rinha.Compilation.Emit;

using LocalVars = Dictionary<VariableSymbol, VariableDefinition>;
using Params = Dictionary<VariableSymbol, (ParameterDefinition param, int index)>;

public partial class Emitter
{
    public void EmitString(ILProcessor il, StringExpr node)
    {
        il.Emit(OpCodes.Ldstr, node.Value);
        il.Emit(OpCodes.Newobj, _knownMethods.GetRef(KnownMethod.RinhaStrCtor));
    }

    public void EmitInteger(ILProcessor il, IntegerExpr node)
    {
        il.Emit(OpCodes.Ldc_I4, node.Value);
        il.Emit(OpCodes.Newobj, _knownMethods.GetRef(KnownMethod.RinhaIntCtor));
    }


    public void EmitBoolean(ILProcessor il, BooleanExpr node)
    {
        il.Emit(OpCodes.Ldc_I4, node.Value ? 1 : 0);
        il.Emit(OpCodes.Newobj, _knownMethods.GetRef(KnownMethod.RinhaBoolCtor));
    }

    public void EmitBinary(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        BinaryExpr node)
    {
        EmitExpression(il, locals, args, node.Lhs);
        EmitExpression(il, locals, args, node.Rhs);
        var method = node.Op switch
        {
            BinaryOp.Add => KnownMethod.RinhaAdd,
            BinaryOp.Sub => KnownMethod.RinhaSub,
            BinaryOp.Mul => KnownMethod.RinhaMul,
            BinaryOp.Div => KnownMethod.RinhaDiv,
            BinaryOp.Rem => KnownMethod.RinhaRem,
            BinaryOp.Eq => KnownMethod.RinhaEq,
            BinaryOp.Neq => KnownMethod.RinhaNeq,
            BinaryOp.Lt => KnownMethod.RinhaLt,
            BinaryOp.Gt => KnownMethod.RinhaGt,
            BinaryOp.Gte => KnownMethod.RinhaGOrEq,
            BinaryOp.Lte => KnownMethod.RinhaLOrEq,
            BinaryOp.And => KnownMethod.RinhaAnd,
            _ => KnownMethod.RinhaOr,
        };
        EmitBuiltInCall(il, method);
    }

    public void EmitTuple(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        TupleLiteralExpr node)
    {
        EmitExpression(il, locals, args, node.First);
        EmitExpression(il, locals, args, node.Second);
        EmitBuiltInCtor(il, KnownMethod.RinhaTupleCtor);
    }

    public void EmuitTupleFirst(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        TupleFirstExpr node)
    {
        EmitExpression(il, locals, args, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaFirst);
    }

    public void EmuitTupleSecond(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        TupleSecondExpr node)
    {
        EmitExpression(il, locals, args, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaSecond);
    }

    public void EmitPrint(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        PrintExpr node)
    {
        EmitExpression(il, locals, args, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaPrint);
    }

    public void EmitIf(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        IfExpr node)
    {
        EmitExpression(il, locals, args, node.Condition);
        EmitBuiltInCall(il, KnownMethod.RinhaGetBoolVal);

        var ifLabel = il.Create(OpCodes.Nop);
        var endLabel = il.Create(OpCodes.Nop);

        il.Emit(OpCodes.Brtrue, ifLabel);
        EmitExpression(il, locals, args, node.Else);
        il.Emit(OpCodes.Br, endLabel);
        il.Append(ifLabel);
        EmitExpression(il, locals, args, node.Then);
        il.Append(endLabel);
    }

    public void EmitLambda(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        LambdaExpr lambda)
    {
        var dependencies = lambda.Scope!.GetOutsideDependencies();

        var ctor = FindClosureCtor(lambda.Symbol);
        il.Emit(OpCodes.Newobj, ctor);
        il.Emit(OpCodes.Ldc_I4, lambda.Parameters.Count + dependencies.Length);
        EmitBuiltInCtor(il, KnownMethod.RinhaClosureCtor);
    }

    public void EmitCall(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        CallExpr node)
    {
        // callee
        EmitExpression(il, locals, args, node.Callee);

        // args
        LambdaExpr? func = null;
        var dependencies = new ImmutableArray<VariableSymbol>();
        if (node.Callee is LambdaExpr)
        {
            func = (LambdaExpr)node.Callee;
        }
        else if (node.Callee is VarExpr)
        {
            var variable = (VarExpr)node.Callee;
            func = _functions[variable.Symbol!];
        }

        if (func is not null)
        {
            dependencies = func.Scope!.GetOutsideDependencies();
        }

        var argsCount = node.Arguments.Count;
        il.Emit(OpCodes.Ldc_I4, argsCount + dependencies.Length);
        il.Emit(OpCodes.Newarr, _knownTypes.GetRef(KnownType.RinhaObject));
        for (var i = 0; i < argsCount; i++)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4, i);
            EmitExpression(il, locals, args, node.Arguments[i]);
            il.Emit(OpCodes.Stelem_Ref);
        }

        for (var i = argsCount; i < argsCount + dependencies.Length; i++)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4, i);
            FindAndEmitVar(il, dependencies[i - argsCount], locals, args);
            il.Emit(OpCodes.Stelem_Ref);
        }

        EmitBuiltInCall(il, KnownMethod.RinhaRunClosure);
    }

    public void EmitLetIn(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        LetInExpr node)
    {
        if (node.Value is LambdaExpr)
        {
            _functions[node.NewVariable] = (LambdaExpr)node.Value;
        }
        EmitExpression(il, locals, args, node.Value);
        var variable = locals![node.NewVariable];
        il.Emit(OpCodes.Stloc, variable);
        EmitExpression(il, locals, args, node.In);
    }


    public void EmitVar(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        VarExpr node)
    {
        FindAndEmitVar(il, node.Symbol, locals, args);
    }

    public void EmitExpression(
        ILProcessor il,
        LocalVars? locals,
        Params? args,
        Expression node)
    {
        switch (node.Kind)
        {
            case BoundKind.String:
                EmitString(il, (StringExpr)node);
                break;

            case BoundKind.Integer:
                EmitInteger(il, (IntegerExpr)node);
                break;

            case BoundKind.Boolean:
                EmitBoolean(il, (BooleanExpr)node);
                break;

            case BoundKind.TupleLiteral:
                EmitTuple(il, locals, args, (TupleLiteralExpr)node);
                break;

            case BoundKind.TupleFirst:
                EmuitTupleFirst(il, locals, args, (TupleFirstExpr)node);
                break;

            case BoundKind.TupleSecond:
                EmuitTupleSecond(il, locals, args, (TupleSecondExpr)node);
                break;

            case BoundKind.Binary:
                EmitBinary(il, locals, args, (BinaryExpr)node);
                break;

            case BoundKind.If:
                EmitIf(il, locals, args, (IfExpr)node);
                break;

            case BoundKind.Print:
                EmitPrint(il, locals, args, (PrintExpr)node);
                break;

            case BoundKind.LetIn:
                EmitLetIn(il, locals, args, (LetInExpr)node);
                break;

            case BoundKind.Var:
                EmitVar(il, locals, args, (VarExpr)node);
                break;

            case BoundKind.Lambda:
                EmitLambda(il, locals, args, (LambdaExpr)node);
                break;

            case BoundKind.Call:
                EmitCall(il, locals, args, (CallExpr)node);
                break;

            default:
                throw new Exception($"Invalid bound expression of kind: {node.Kind}");
        }
    }

    private void EmitBuiltInCall(ILProcessor il, KnownMethod method)
    {
        il.Emit(OpCodes.Call, _knownMethods.GetRef(method));
    }

    private void EmitBuiltInCtor(ILProcessor il, KnownMethod method)
    {
        il.Emit(OpCodes.Newobj, _knownMethods.GetRef(method));
    }

    private void FindAndEmitVar(
        ILProcessor il,
        VariableSymbol? symbol,
        LocalVars? locals,
        Params? args)
    {
        VariableDefinition? variable;
        if (locals!.TryGetValue(symbol!, out variable))
        {
            il.Emit(OpCodes.Ldloc, variable);
        }
        else
        {
            var arg = args![symbol!];
            il.Emit(OpCodes.Ldarg, arg.param);
            il.Emit(OpCodes.Ldc_I4, arg.index);
            il.Emit(OpCodes.Ldelem_Ref);
        }
    }
}
