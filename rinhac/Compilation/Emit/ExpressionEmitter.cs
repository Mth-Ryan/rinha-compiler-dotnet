using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;
using Rinha.Syntax;

namespace Rinha.Compilation.Emit;

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
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        BinaryExpr node)
    {
        EmitExpression(il, locals, node.Lhs);
        EmitExpression(il, locals, node.Rhs);
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
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        TupleLiteralExpr node)
    {
        EmitExpression(il, locals, node.First);
        EmitExpression(il, locals, node.Second);
        EmitBuiltInCtor(il, KnownMethod.RinhaTupleCtor);
    }

    public void EmuitTupleFirst(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        TupleFirstExpr node)
    {
        EmitExpression(il, locals, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaFirst);
    }

    public void EmuitTupleSecond(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        TupleSecondExpr node)
    {
        EmitExpression(il, locals, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaSecond);
    }

    public void EmitPrint(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        PrintExpr node)
    {
        EmitExpression(il, locals, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaPrint);
    }

    public void EmitIf(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        IfExpr node)
    {
        EmitExpression(il, locals, node.Condition);
        EmitBuiltInCall(il, KnownMethod.RinhaGetBoolVal);

        var ifLabel = il.Create(OpCodes.Nop);
        var endLabel = il.Create(OpCodes.Nop);

        il.Emit(OpCodes.Brtrue, ifLabel);
        EmitExpression(il, locals, node.Else);
        il.Emit(OpCodes.Br, endLabel);
        il.Append(ifLabel);
        EmitExpression(il, locals, node.Then);
        il.Append(endLabel);
    }

    public void EmitLetIn(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        LetInExpr node)
    {
        EmitExpression(il, locals, node.Value);
        var variable = locals![node.NewVariable];
        il.Emit(OpCodes.Stloc, variable);
        EmitExpression(il, locals, node.In);
    }


    public void EmitVar(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
        VarExpr node)
    {
        var variable = locals![node.Symbol!];
        il.Emit(OpCodes.Ldloc, variable);
    }

    public void EmitExpression(
        ILProcessor il,
        Dictionary<VariableSymbol, VariableDefinition>? locals,
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
                EmitTuple(il, locals, (TupleLiteralExpr)node);
                break;

            case BoundKind.TupleFirst:
                EmuitTupleFirst(il, locals, (TupleFirstExpr)node);
                break;

            case BoundKind.TupleSecond:
                EmuitTupleSecond(il, locals, (TupleSecondExpr)node);
                break;

            case BoundKind.Binary:
                EmitBinary(il, locals, (BinaryExpr)node);
                break;

            case BoundKind.If:
                EmitIf(il, locals, (IfExpr)node);
                break;

            case BoundKind.Print:
                EmitPrint(il, locals, (PrintExpr)node);
                break;

            case BoundKind.LetIn:
                EmitLetIn(il, locals, (LetInExpr)node);
                break;

            case BoundKind.Var:
                EmitVar(il, locals, (VarExpr)node);
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
}
