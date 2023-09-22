namespace Rinha.Semantic.BoundTree;

public class CallExpr : Expression
{
    public override BoundKind Kind => BoundKind.Call;

    public required Expression Callee { get; init; }
    public required List<Expression> Arguments { get; init; }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Callee;
        foreach (var arg in Arguments)
            yield return arg;
    }
}
