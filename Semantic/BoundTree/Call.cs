namespace Rinha.Semantic.BoundTree;

public class CallExpr : Expression
{
    public override BoundKind Kind => BoundKind.Call;

    public required Expression Callee { get; init; }
    public required List<Expression> Arguments { get; init; }
}
