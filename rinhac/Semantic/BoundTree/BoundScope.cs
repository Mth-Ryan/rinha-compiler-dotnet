using System.Collections.Immutable;

namespace Rinha.Semantic.BoundTree;

public enum VariableSymbolAccess
{
    Inner,
    Outter,
}

public enum VariableSymbolKind
{
    Argument,
    Variable,
}

public class VariableSymbol
{
    public string Name { get; set; }
    public VariableSymbolKind Kind { get; set; }

    public VariableSymbol(string name, VariableSymbolKind kind)
    {
        Name = name;
        Kind = kind;
    }
}

public class VariableLookUpResponse
{
    public VariableSymbol Variable { get; set; }
    public VariableSymbolAccess Access { get; set; }

    public VariableLookUpResponse(VariableSymbol variable, VariableSymbolAccess access)
    {
        Variable = variable;
        Access = access;
    }
}

public enum ScopeKind
{
    Block,
    Closure
}

public class BoundScope
{
    private Dictionary<string, VariableSymbol> _variables;

    internal BoundScope? Parent { get; init; }
    internal List<BoundScope> Children { get; set; }

    public ScopeKind Kind { get; init; }

    public BoundScope(BoundScope? parent, ScopeKind kind)
    {
        Kind = kind;
        Children = new List<BoundScope>();
        Parent = parent;

        if (Parent is not null)
        {
            Parent.Children.Add(this);
        }

        _variables = new Dictionary<string, VariableSymbol>();
    }

    public bool TryDeclare(VariableSymbol variable)
    {
        if (_variables.ContainsKey(variable.Name))
            return false;

        _variables.Add(variable.Name, variable);
        return true;
    }

    public VariableLookUpResponse? TryLookUp(string name)
    {
        VariableSymbol? variable;
        if (_variables.TryGetValue(name, out variable))
            return new VariableLookUpResponse(variable, VariableSymbolAccess.Inner);

        if (Parent is null)
            return null;

        var parentResponse = Parent.TryLookUp(name);
        if (parentResponse is null)
            return null;

        return new VariableLookUpResponse(
            parentResponse.Variable,
            VariableSymbolAccess.Outter);
    }

    public ImmutableArray<VariableSymbol> GetAllBlockVariables()
    {
        var variables = _variables.Values
            .Where(x => x.Kind == VariableSymbolKind.Variable)
            .ToList();

        var blockChildren = Children.Where(c => c.Kind == ScopeKind.Block);
        foreach (var child in blockChildren)
        {
            variables.AddRange(child.GetAllBlockVariables());
        }

        return variables.ToImmutableArray();
    }

    public ImmutableArray<VariableSymbol> GetAllArguments() =>
        _variables.Values
            .Where(x => x.Kind == VariableSymbolKind.Argument)
            .ToImmutableArray();
}


