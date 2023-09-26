namespace Rinha.Semantic.BoundTree;

public class LambdaExpr : Expression
{
    public override BoundKind Kind => BoundKind.Lambda;

    public required BoundScope? Scope { get; set; }
    public required List<string> Parameters { get; init; }
    public required Expression Body { get; init; }

    public override IEnumerable<Node>? GetChildren()
    {
        yield return Body;
    }

    public override string ToString()
    {
        var image = $"{Kind.ToString()}";
        if (Parameters.Count > 0)
        {
            image += " (";
            for (int i = 0; i < Parameters.Count - 1; i++)
            {
                image += $"{Parameters[i]}, ";
            }
            image += Parameters[Parameters.Count - 1];
            image += ")";
        }
        return image;
    }
}
