using Mono.Cecil.Cil;
using Rinha.Semantic.BoundTree;

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

    public void EmitPrint(ILProcessor il, PrintExpr node)
    {
        EmitExpression(il, node.Value);
        il.Emit(OpCodes.Call, _knownMethods.GetRef(KnownMethod.RinhaPrint));
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
