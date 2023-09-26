using System.Collections.Immutable;
using Rinha.Diagnostics;
using Rinha.Semantic.BoundTree;
using Rinha.Syntax;

namespace Rinha.Semantic;

public class Binder
{
    private readonly DiagnosticsBag _diagnostics;
    private readonly BoundScope _globalScope;

    public Binder()
    {
        _diagnostics = new DiagnosticsBag();
        _globalScope = new BoundScope(null, ScopeKind.Closure);
    }

    public (Expression?, ImmutableArray<Diagnostic>) Bind(AstFile ast)
    {
        var expr = BindExpr(ast.Expression, _globalScope);
        return (expr, _diagnostics.ToImmutableArray());
    }

    private IntegerExpr BindInteger(IntTerm node) =>
        new IntegerExpr { Value = node.Value };

    private BooleanExpr BindBool(BoolTerm node) =>
        new BooleanExpr { Value = node.Value };

    private StringExpr BindString(StrTerm node) =>
        new StringExpr { Value = node.Value };

    private BinaryExpr BindBinary(BinaryTerm node, BoundScope? scope) =>
        new BinaryExpr
        {
            Lhs = BindExpr(node.Lhs, scope),
            Op = node.Op,
            Rhs = BindExpr(node.Rhs, scope)
        };

    private CallExpr BindCall(CallTerm node, BoundScope? scope) =>
        // Validate the callee
        new CallExpr
        {
            Callee = BindExpr(node.Callee, scope),
            Arguments = node.Arguments
                .Select(x => BindExpr(x, scope)).ToList()
        };

    private LetIntExpr BindLetInt(LetTerm node, BoundScope? scope)
    {
        var newScope = new BoundScope(scope, ScopeKind.Block);
        newScope.TryDeclare(new VariableSymbol(
            node.Name.Text,
            VariableSymbolKind.Variable));

        return new LetIntExpr
        {
            Scope = newScope,
            Name = node.Name.Text,
            Value = BindExpr(node.Value, newScope),
            In = BindExpr(node.Next, newScope)
        };
    }

    private LambdaExpr BindLambda(FunctionTerm node, BoundScope? scope)
    {
        var newScope = new BoundScope(scope, ScopeKind.Closure);
        foreach (var param in node.Parameters)
        {
            newScope.TryDeclare(new VariableSymbol(
                param.Text,
                VariableSymbolKind.Argument));
        }

        return new LambdaExpr
        {
            Scope = newScope,
            Parameters = node.Parameters.Select(x => x.Text).ToList(),
            Body = BindExpr(node.Value, newScope)
        };
    }

    private IfExpr BindIf(IfTerm node, BoundScope? scope) =>
        // validate condition
        new IfExpr
        {
            Condition = BindExpr(node.Condition, scope),
            Then = BindExpr(node.Then, scope),
            Else = BindExpr(node.Otherwise, scope)
        };

    private TupleFirstExpr BindTupleFirst(FirstTerm node, BoundScope? scope) =>
        new TupleFirstExpr
        {
            Value = BindExpr(node.Value, scope)
        };

    private TupleSecondExpr BindTupleSecond(SecondTerm node, BoundScope? scope) =>
        new TupleSecondExpr
        {
            Value = BindExpr(node.Value, scope)
        };


    private TupleLiteralExpr BindTupleLiteral(TupleTerm node, BoundScope? scope) =>
        new TupleLiteralExpr
        {
            First = BindExpr(node.First, scope),
            Second = BindExpr(node.Second, scope)
        };

    private PrintExpr BindPrint(PrintTerm node, BoundScope? scope) =>
        new PrintExpr
        {
            Value = BindExpr(node.Value, scope)
        };

    private VarExpr BindVar(VarTerm node, BoundScope? scope)
    {
        VariableSymbol? symbol = null;
        VariableSymbolAccess access = VariableSymbolAccess.Inner;

        if (scope is not null)
        {
            var symbolResponse = scope.TryLookUp(node.Text);
            if (symbolResponse is not null)
            {
                symbol = symbolResponse.Variable;
                access = symbolResponse.Access;
            }
        }

        return new VarExpr
        {
            Symbol = symbol,
            Access = access,
            Name = node.Text
        };
    }

    private Expression BindExpr(Term node, BoundScope? scope)
    {
        return node switch
        {
            IntTerm n => BindInteger(n),
            StrTerm n => BindString(n),
            BoolTerm n => BindBool(n),
            VarTerm n => BindVar(n, scope),
            FirstTerm n => BindTupleFirst(n, scope),
            SecondTerm n => BindTupleSecond(n, scope),
            TupleTerm n => BindTupleLiteral(n, scope),
            PrintTerm n => BindPrint(n, scope),
            FunctionTerm n => BindLambda(n, scope),
            CallTerm n => BindCall(n, scope),
            BinaryTerm n => BindBinary(n, scope),
            IfTerm n => BindIf(n, scope),
            LetTerm n => BindLetInt(n, scope),
            // temporary
            _ => new InvalidExpr {}
        };
    }
}
