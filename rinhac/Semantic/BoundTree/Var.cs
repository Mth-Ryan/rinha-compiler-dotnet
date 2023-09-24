namespace Rinha.Semantic.BoundTree;

public class VarExpr : Expression
{
    public override BoundKind Kind => BoundKind.Var;

    public required string Name { get; init; }

    public override IEnumerable<Node>? GetChildren() => null;

    public override string ToString()
    {
        return $"{Kind.ToString()} {Name}";
    }
}
