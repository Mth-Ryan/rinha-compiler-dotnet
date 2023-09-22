namespace Rinha.Semantic.BoundTree;

public class TupleFirstExpr : Expression
{
    public override BoundKind Kind => BoundKind.TupleFirst;

    public required Expression Value { get; init; }

    public override IEnumerable<Node>? GetChildren()
    {
        yield return Value;
    }
}
