namespace Rinha.Syntax;

public class Parameter
{
    public required Location Location { get; set; }
    public required string Text { get; set; }
}

public abstract class Term : AstNode
{
    public required string Kind { get; set; }
}

public sealed class IntTerm : Term
{
    public int Value { get; set; }
}

public sealed class StrTerm : Term
{
    public required string Value { get; set; }
}

public sealed class BoolTerm : Term
{
    public bool Value { get; set; }
}

public sealed class CallTerm : Term
{
    public required Term Callee { get; set; }
    public required List<Term> Arguments { get; set; }
}

public sealed class BinaryTerm : Term
{
    public required Term Lhs { get; set; }
    public required string Op { get; set; }
    public required Term Rhs { get; set; }
}

public sealed class FunctionTerm : Term
{
    public required List<Parameter> Parameters { get; set; }
    public required Term Value { get; set; }
}

public sealed class LetTerm : Term
{
    public required Parameter Name { get; set; }
    public required Term Value { get; set; }
    public required Term Next { get; set; }
}

public sealed class IfTerm : Term
{
    public required Term Condition { get; set; }
    public required Term Then { get; set; }
    public required Term Otherwise { get; set; }
}

public sealed class Print : Term
{
    public required Term Value { get; set; }
}

public sealed class First : Term
{
    public required Term Value { get; set; }
}

public sealed class Secound : Term
{
    public required Term Value { get; set; }
}

public sealed class Tuple : Term
{
    public required Term First { get; set; }
    public required Term Second { get; set; }
}

public sealed class Var : Term
{
    public required string Text { get; set; }
}
