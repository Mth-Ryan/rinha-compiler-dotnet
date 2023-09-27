using System.Collections.Immutable;
using Rinha.Semantic.BoundTree;

namespace Rinha.Semantic;

public class BoundProgram
{
    public Expression BoundTree { get; init; }
    public BoundScope GlobalScope { get; init; }
    public ImmutableArray<LambdaExpr> Functions { get; init; }

    public BoundProgram(
        Expression boundTree,
        BoundScope globalScope,
        ImmutableArray<LambdaExpr> functions)
    {
        BoundTree = boundTree;
        GlobalScope = globalScope;
        Functions = functions;
    }
}
