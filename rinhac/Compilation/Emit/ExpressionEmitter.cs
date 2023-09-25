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

    public void EmitBinary(ILProcessor il, BinaryExpr node)
    {
        EmitExpression(il, node.Lhs);
        EmitExpression(il, node.Rhs);
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

    public void EmitTuple(ILProcessor il, TupleLiteralExpr node)
    {
        EmitExpression(il, node.First);
        EmitExpression(il, node.Second);
        EmitBuiltInCtor(il, KnownMethod.RinhaTupleCtor);
    }

    public void EmuitTupleFirst(ILProcessor il, TupleFirstExpr node)
    {
        EmitExpression(il, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaFirst);
    }

    public void EmuitTupleSecond(ILProcessor il, TupleSecondExpr node)
    {
        EmitExpression(il, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaSecond);
    }

    public void EmitPrint(ILProcessor il, PrintExpr node)
    {
        EmitExpression(il, node.Value);
        EmitBuiltInCall(il, KnownMethod.RinhaPrint);
    }

    public void EmitExpression(ILProcessor il, Expression node)
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
                EmitTuple(il, (TupleLiteralExpr)node);
                break;

            case BoundKind.TupleFirst:
                EmuitTupleFirst(il, (TupleFirstExpr)node);
                break;

            case BoundKind.TupleSecond:
                EmuitTupleSecond(il, (TupleSecondExpr)node);
                break;

            case BoundKind.Binary:
                EmitBinary(il, (BinaryExpr)node);
                break;

            case BoundKind.Print:
                EmitPrint(il, (PrintExpr)node);
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
