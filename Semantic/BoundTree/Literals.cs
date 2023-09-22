namespace Rinha.Semantic.BoundTree;

public abstract class Literal : Expression
{
}

public class IntegerExpr : Literal
{
    public override BoundKind Kind => BoundKind.Integer;
    public required int Value { get; init; }
}

public class StringExpr : Literal
{
    public override BoundKind Kind => BoundKind.Boolean;
    public required string Value { get; init; }
}


public class BooleanExpr : Literal
{
    public override BoundKind Kind => BoundKind.Boolean;
    public required bool Value { get; init; }
}
