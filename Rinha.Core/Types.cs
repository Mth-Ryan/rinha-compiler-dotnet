namespace Rinha.Core;

public enum RinhaObjKind
{
    Int,
    Str,
    Bool,

    Tuple,
    Closure,
}

public abstract class RinhaObject
{
    public abstract RinhaObjKind Kind { get; }
}

public class RinhaInt : RinhaObject
{
    public override RinhaObjKind Kind => RinhaObjKind.Int;
    public int Value { get; set; }

    public RinhaInt(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class RinhaBool : RinhaObject
{
    public override RinhaObjKind Kind => RinhaObjKind.Bool;
    public bool Value { get; set; }

    public RinhaBool(bool value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class RinhaStr : RinhaObject
{
    public override RinhaObjKind Kind => RinhaObjKind.Str;
    public string Value { get; set; }

    public RinhaStr(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}

public class RinhaTuple : RinhaObject
{
    public override RinhaObjKind Kind => RinhaObjKind.Tuple;

    public (RinhaObject, RinhaObject) Value { get; set; }

    public RinhaTuple(RinhaObject first, RinhaObject last)
    {
        Value = (first, last);
    }

    public override string ToString()
    {
        return $"({Value.Item1.ToString()}, {Value.Item2.ToString()})";
    }
}

public class RinhaClosureParams
{
    public RinhaObject[] Params { get; set; }

    public RinhaClosureParams(RinhaObject[] parameters)
    {
        Params = parameters;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var i in Params)
            {
                hash = hash * 31 + i.GetHashCode();
            }

            return hash;
        }
    }
}

public class RinhaClosure : RinhaObject
{
    public override RinhaObjKind Kind => RinhaObjKind.Closure;
    
    public Func<RinhaClosureParams, RinhaObject>? Value { get; set; }
    public int ParamsCount { get; set; }

    public RinhaClosure(int paramsCount)
    {
        ParamsCount = paramsCount;
    }

    public override string ToString()
    {
        return "<#closure>";
    }
}
