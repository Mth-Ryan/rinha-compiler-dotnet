namespace Rinha.Core;

public static partial class BuiltInMethods
{
    public static RinhaObject Add(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind == RinhaObjKind.Int && rhs.Kind == RinhaObjKind.Int)
        {
            return new RinhaInt(((RinhaInt)lhs).Value + ((RinhaInt)rhs).Value);
        }
        else if (lhs.Kind == RinhaObjKind.Int && rhs.Kind == RinhaObjKind.Str)
        {
            return new RinhaStr(((RinhaInt)lhs).Value.ToString() + ((RinhaStr)rhs).Value);
        }
        else if (lhs.Kind == RinhaObjKind.Str && rhs.Kind == RinhaObjKind.Int)
        {
            return new RinhaStr(((RinhaStr)lhs).Value + ((RinhaInt)rhs).Value.ToString());
        }
        else if (lhs.Kind == RinhaObjKind.Str && rhs.Kind == RinhaObjKind.Str)
        {
            return new RinhaStr(((RinhaStr)lhs).Value + ((RinhaStr)rhs).Value);
        }

        throw new InvalidOperationException();
    }

    public static RinhaObject Sub(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }
        return new RinhaInt(((RinhaInt)lhs).Value - ((RinhaInt)rhs).Value);
    }

    public static RinhaObject Mul(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }
        return new RinhaInt(((RinhaInt)lhs).Value * ((RinhaInt)rhs).Value);
    }

    public static RinhaObject Div(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }
        return new RinhaInt(((RinhaInt)lhs).Value / ((RinhaInt)rhs).Value);
    }

    public static RinhaObject Rem(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }
        return new RinhaInt(((RinhaInt)lhs).Value % ((RinhaInt)rhs).Value);
    }
}
