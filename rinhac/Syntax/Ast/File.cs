namespace Rinha.Syntax;

public sealed class AstFile : AstNode
{
    public required string Name { get; set; }
    public required Term Expression { get; set; }
}
