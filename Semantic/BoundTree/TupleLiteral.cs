namespace Rinha.Semantic.BoundTree;

public class TupleLiteralExpr : Expression
{
    public override BoundKind Kind => BoundKind.TupleLiteral;

    public required Expression First { get; init; }
    public required Expression Second { get; init; }
}
