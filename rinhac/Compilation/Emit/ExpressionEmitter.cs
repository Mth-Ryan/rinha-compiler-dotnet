using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;

namespace Rinha.Compilation.Emit;

public partial class Emitter
{
    public void EmitString(ILProcessor il, StringExpr node)
    {
        il.Emit(OpCodes.Ldstr, node.Value);
    }

    public void EmitInteger(ILProcessor il, IntegerExpr node)
    {
        il.Emit(OpCodes.Ldc_I4, node.Value);
    }


    public void EmitBoolean(ILProcessor il, BooleanExpr node)
    {
        il.Emit(OpCodes.Ldc_I4, node.Value ? 1 : 0);
    }

    public void EmitPrint(ILProcessor il, PrintExpr node)
    {

        var writelineRef = _module
            .ImportReference(typeof(Console)
            .GetMethod("WriteLine", new [] { typeof(object) }));

        EmitExpression(il, node.Value);
        il.Emit(OpCodes.Call, writelineRef);
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

            case BoundKind.Print:
                EmitPrint(il, (PrintExpr)node);
                break;
        }
    }
}
