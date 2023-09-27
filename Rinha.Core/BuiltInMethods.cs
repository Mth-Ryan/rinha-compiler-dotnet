namespace Rinha.Core;

public static partial class BuiltInMethods
{
    public static RinhaObject First(RinhaObject tuple)
    {
        if (tuple.Kind != RinhaObjKind.Tuple)
        {
            throw new InvalidOperationException();
        }
        return ((RinhaTuple)tuple).Value.Item1;
    }

    public static RinhaObject Second(RinhaObject tuple)
    {
        if (tuple.Kind != RinhaObjKind.Tuple)
        {
            throw new InvalidOperationException();
        }
        return ((RinhaTuple)tuple).Value.Item2;
    }

    public static RinhaObject Print(RinhaObject obj)
    {
        Console.WriteLine(obj.ToString());
        return obj;
    }

    public static bool GetBoolValue(RinhaObject obj)
    {
        if (obj.Kind != RinhaObjKind.Bool)
        {
            throw new InvalidOperationException();
        }
        return ((RinhaBool)obj).Value;
    }

    public static RinhaObject RunClosure(RinhaObject closure, RinhaObject[] args)
    {
        if (closure.Kind != RinhaObjKind.Closure)
        {
            throw new InvalidOperationException();
        }

        var cls = (RinhaClosure)closure;

        if (args.Length != cls.ParamsCount)
        {
            throw new ArgumentException("wrong number of arguments");
        }

        return cls.Inner.Run(args);
    }
}
