using Rinha.Semantic.BoundTree;

namespace Rinha.Semantic;

public class BoundProgram
{
    public Expression BoundTree { get; init; }
    public BoundScope GlobalScope { get; init; }

    public BoundProgram(Expression boundTree, BoundScope globalScope)
    {
        BoundTree = boundTree;
        GlobalScope = globalScope;
    }
}
