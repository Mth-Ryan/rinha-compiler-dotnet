namespace Rinha.Semantic.BoundTree;

public class IfExpr : Expression
{
    public override BoundKind Kind => BoundKind.If;

    public required Expression Condition { get; init; }
    public required Expression Then { get; init; }
    public required Expression Else { get; init; }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Condition;
        yield return Then;
        yield return Else;
    }
}
