namespace Rinha.Semantic.BoundTree;

public class LetInExpr : Expression
{
    public override BoundKind Kind => BoundKind.LetIn;

    public required BoundScope? Scope { get; set; }
    public required VariableSymbol NewVariable { get; set; }
    public required string Name { get; init; }
    public required Expression Value { get; init; }
    public required Expression In { get; init; }

    public override IEnumerable<Node>? GetChildren()
    {
        yield return Value;
        yield return In;
    }

    public override string ToString()
    {
        return $"{Kind.ToString()} {Name}";
    }
}
