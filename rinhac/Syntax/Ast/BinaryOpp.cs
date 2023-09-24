using System.Text.Json.Serialization;

namespace Rinha.Syntax;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BinaryOp
{
    Add,
    Sub,
    Mul,
    Div,
    Rem,
    Eq,
    Neq,
    Lt,
    Gt,
    Lte,
    Gte,
    And,
    Or
}
