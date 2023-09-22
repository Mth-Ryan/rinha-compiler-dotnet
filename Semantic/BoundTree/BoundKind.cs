namespace Rinha.Semantic.BoundTree;

public enum BoundKind
{
    // constant
    Integer,
    String,
    Boolean,

    // expressions
    Call,
    Binary,
    Lambda,
    LetIn,
    If,
    Print,
    TupleFirst,
    TupleSecond,
    TupleLiteral,
    Var
}
