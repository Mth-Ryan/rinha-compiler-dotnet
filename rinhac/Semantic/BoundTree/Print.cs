namespace Rinha.Semantic.BoundTree;

public class PrintExpr : Expression
{
    public override BoundKind Kind => BoundKind.Print;

    public required Expression Value { get; init; }

    public override IEnumerable<Node>? GetChildren()
    {
        yield return Value;
    }
}
