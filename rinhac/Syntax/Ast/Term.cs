using System.Text.Json.Serialization;

namespace Rinha.Syntax;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(IntTerm), typeDiscriminator: "Int")]
[JsonDerivedType(typeof(StrTerm), typeDiscriminator: "Str")]
[JsonDerivedType(typeof(BoolTerm), typeDiscriminator: "Bool")]
[JsonDerivedType(typeof(CallTerm), typeDiscriminator: "Call")]
[JsonDerivedType(typeof(BinaryTerm), typeDiscriminator: "Binary")]
[JsonDerivedType(typeof(FunctionTerm), typeDiscriminator: "Function")]
[JsonDerivedType(typeof(LetTerm), typeDiscriminator: "Let")]
[JsonDerivedType(typeof(IfTerm), typeDiscriminator: "If")]
[JsonDerivedType(typeof(PrintTerm), typeDiscriminator: "Print")]
[JsonDerivedType(typeof(FirstTerm), typeDiscriminator: "First")]
[JsonDerivedType(typeof(SecondTerm), typeDiscriminator: "Second")]
[JsonDerivedType(typeof(TupleTerm), typeDiscriminator: "Tuple")]
[JsonDerivedType(typeof(VarTerm), typeDiscriminator: "Var")]
public class Term : AstNode {}

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
    public required BinaryOp Op { get; set; }
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

public sealed class PrintTerm : Term
{
    public required Term Value { get; set; }
}

public sealed class FirstTerm : Term
{
    public required Term Value { get; set; }
}

public sealed class SecondTerm : Term
{
    public required Term Value { get; set; }
}

public sealed class TupleTerm : Term
{
    public required Term First { get; set; }
    public required Term Second { get; set; }
}

public sealed class VarTerm : Term
{
    public required string Text { get; set; }
}
