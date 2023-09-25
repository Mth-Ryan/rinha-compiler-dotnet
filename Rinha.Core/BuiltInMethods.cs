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

    public static RinhaObject AddClosureBody(RinhaObject closure, Func<RinhaClosureParams, RinhaObject> body)
    {
        if (closure.Kind != RinhaObjKind.Closure)
        {
            throw new InvalidOperationException();
        }

        var cls = (RinhaClosure)closure;
        cls.Value = body;

        return cls;
    }

    public static RinhaObject RunClosure(RinhaObject closure, RinhaClosureParams args)
    {
        if (closure.Kind != RinhaObjKind.Closure)
        {
            throw new InvalidOperationException();
        }

        var cls = (RinhaClosure)closure;

        if (cls.Value is null)
        {
            throw new Exception("use of function not yet initialized");
        }

        if (args.Params.Length != cls.ParamsCount)
        {
            throw new ArgumentException("wrong number of arguments");
        }

        return cls.Value(args);
    }
}
