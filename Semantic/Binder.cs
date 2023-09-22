using System.Collections.Immutable;
using Rinha.Diagnostics;
using Rinha.Semantic.BoundTree;
using Rinha.Syntax;

namespace Rinha.Semantic;

public class Binder
{
    private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();

    public Binder()
    {
    }

    public (Expression?, ImmutableArray<Diagnostic>) Bind(AstFile ast)
    {
        var expr = BindExpr(ast.Expression);
        return (expr, _diagnostics.ToImmutableArray());
    }

    private IntegerExpr BindInteger(IntTerm node) =>
        new IntegerExpr { Value = node.Value };

    private BooleanExpr BindBool(BoolTerm node) =>
        new BooleanExpr { Value = node.Value };

    private StringExpr BindString(StrTerm node) =>
        new StringExpr { Value = node.Value };

    private BinaryExpr BindBinary(BinaryTerm node) =>
        new BinaryExpr
        {
            Lhs = BindExpr(node.Lhs),
            Op = node.Op,
            Rhs = BindExpr(node.Rhs)
        };

    private CallExpr BindCall(CallTerm node) =>
        // Validate the callee
        new CallExpr
        {
            Callee = BindExpr(node.Callee),
            Arguments = node.Arguments
                .Select(x => BindExpr(x)).ToList()
        };

    private LetIntExpr BindLetInt(LetTerm node) =>
        new LetIntExpr
        {
            Name = node.Name.Text,
            Value = BindExpr(node.Value),
            In = BindExpr(node.Next)
        };

    private LambdaExpr BindLambda(FunctionTerm node) =>
        new LambdaExpr
        {
            Parameters = node.Parameters.Select(x => x.Text).ToList(),
            Body = BindExpr(node.Value)
        };

    private IfExpr BindIf(IfTerm node) =>
        // validate condition
        new IfExpr
        {
            Condition = BindExpr(node.Condition),
            Then = BindExpr(node.Then),
            Else = BindExpr(node.Otherwise)
        };

    private TupleFirstExpr BindTupleFirst(FirstTerm node) =>
        new TupleFirstExpr
        {
            Value = BindExpr(node.Value)
        };

    private TupleSecondExpr BindTupleSecond(SecondTerm node) =>
        new TupleSecondExpr
        {
            Value = BindExpr(node.Value)
        };


    private TupleLiteralExpr BindTupleLiteral(TupleTerm node) =>
        new TupleLiteralExpr
        {
            First = BindExpr(node.First),
            Second = BindExpr(node.Second)
        };

    private PrintExpr BindPrint(PrintTerm node) =>
        new PrintExpr
        {
            Value = BindExpr(node.Value)
        };

    private VarExpr BindVar(VarTerm node) =>
        new VarExpr
        {
            Name = node.Text
        };

    private Expression BindExpr(Term node)
    {
        return node switch
        {
            IntTerm n => BindInteger(n),
            StrTerm n => BindString(n),
            BoolTerm n => BindBool(n),
            VarTerm n => BindVar(n),
            FirstTerm n => BindTupleFirst(n),
            SecondTerm n => BindTupleSecond(n),
            TupleTerm n => BindTupleLiteral(n),
            PrintTerm n => BindPrint(n),
            FunctionTerm n => BindLambda(n),
            CallTerm n => BindCall(n),
            BinaryTerm n => BindBinary(n),
            IfTerm n => BindIf(n),
            LetTerm n => BindLetInt(n),
            // temporary
            _ => new InvalidExpr {}
        };
    }
}
