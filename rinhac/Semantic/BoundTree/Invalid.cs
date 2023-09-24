namespace Rinha.Semantic.BoundTree;

// temporary
public class InvalidExpr : Expression
{
    public override BoundKind Kind => BoundKind.Print;

    public override IEnumerable<Node>? GetChildren() => null;
}
