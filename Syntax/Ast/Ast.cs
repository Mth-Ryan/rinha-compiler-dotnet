namespace Rinha.Syntax;

public class Location
{
    public required uint Start { get; set; }
    public required uint End { get; set; }
    public required string Filename { get; set; }
}

public abstract class AstNode 
{
    public required Location Location { get; set; }
}
