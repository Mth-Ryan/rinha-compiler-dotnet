namespace Rinha.Semantic.BoundTree;

public class LetIntExpr : Expression
{
    public override BoundKind Kind => BoundKind.LetIn;

    public required string Name { get; init; }
    public required Expression Value { get; init; }
    public required Expression In { get; init; }
}
