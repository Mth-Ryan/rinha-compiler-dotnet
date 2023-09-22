namespace Rinha.Semantic.BoundTree;

public abstract class Node
{
    public abstract BoundKind Kind { get; }

    public abstract IEnumerable<Node>? GetChildren();
}
