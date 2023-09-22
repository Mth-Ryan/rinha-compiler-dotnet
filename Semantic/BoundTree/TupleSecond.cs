namespace Rinha.Semantic.BoundTree;

public class TupleSecondExpr : Expression
{
    public override BoundKind Kind => BoundKind.TupleSecond;

    public required Expression Value { get; init; }
}
