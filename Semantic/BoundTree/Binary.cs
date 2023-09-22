using Rinha.Syntax;

namespace Rinha.Semantic.BoundTree;

public class BinaryExpr : Expression
{
    public override BoundKind Kind => BoundKind.Binary;

    public required Expression Lhs { get; init; }
    public required BinaryOp Op { get; init; }
    public required Expression Rhs { get; init; }
}