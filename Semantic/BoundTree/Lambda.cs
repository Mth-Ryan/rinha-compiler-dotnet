namespace Rinha.Semantic.BoundTree;

public class LambdaExpr : Expression
{
    public override BoundKind Kind => BoundKind.Lambda;

    public required List<string> Parameters { get; init; }
    public required Expression Body { get; init; }
}
