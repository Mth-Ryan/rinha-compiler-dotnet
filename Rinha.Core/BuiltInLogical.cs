namespace Rinha.Core;

public static partial class BuiltInMethods
{
    public static RinhaObject Eq(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != rhs.Kind)
        {
            throw new InvalidOperationException();
        }

        switch (lhs.Kind)
        {
            case RinhaObjKind.Int:
                return new RinhaBool(((RinhaInt)lhs).Value == ((RinhaInt)rhs).Value);

            case RinhaObjKind.Bool:
                return new RinhaBool(((RinhaBool)lhs).Value == ((RinhaBool)rhs).Value);

            case RinhaObjKind.Str:
                return new RinhaBool(String.Equals(((RinhaStr)lhs).Value,
                                                   ((RinhaStr)rhs).Value));

            default:
                throw new InvalidOperationException();
        }
    }

    public static RinhaObject NotEq(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != rhs.Kind)
        {
            throw new InvalidOperationException();
        }

        switch (lhs.Kind)
        {
            case RinhaObjKind.Int:
                return new RinhaBool(((RinhaInt)lhs).Value != ((RinhaInt)rhs).Value);

            case RinhaObjKind.Bool:
                return new RinhaBool(((RinhaBool)lhs).Value != ((RinhaBool)rhs).Value);

            case RinhaObjKind.Str:
                return new RinhaBool(!String.Equals(((RinhaStr)lhs).Value,
                                                   ((RinhaStr)rhs).Value));

            default:
                throw new InvalidOperationException();
        }
    }

    public static RinhaObject LessThan(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaInt)lhs).Value < ((RinhaInt)rhs).Value);
    }

    public static RinhaObject GreaterThan(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaInt)lhs).Value > ((RinhaInt)rhs).Value);
    }

    public static RinhaObject LessOrEq(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaInt)lhs).Value <= ((RinhaInt)rhs).Value);
    }

    public static RinhaObject GreaterOrEq(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Int || rhs.Kind != RinhaObjKind.Int)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaInt)lhs).Value >= ((RinhaInt)rhs).Value);
    }

    public static RinhaObject And(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Bool || rhs.Kind != RinhaObjKind.Bool)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaBool)lhs).Value && ((RinhaBool)rhs).Value);
    }

    public static RinhaObject Or(RinhaObject lhs, RinhaObject rhs)
    {
        if (lhs.Kind != RinhaObjKind.Bool || rhs.Kind != RinhaObjKind.Bool)
        {
            throw new InvalidOperationException();
        }

        return new RinhaBool(((RinhaBool)lhs).Value || ((RinhaBool)rhs).Value);
    }
}
