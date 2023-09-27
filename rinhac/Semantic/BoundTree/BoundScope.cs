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

public class FunctionSymbol
{
    public FunctionSymbol()
    {
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
    private List<VariableSymbol> _used;

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
        _used = new List<VariableSymbol>();
    }

    public bool TryDeclare(VariableSymbol variable)
    {
        if (_variables.ContainsKey(variable.Name))
            return false;

        _variables.Add(variable.Name, variable);
        return true;
    }

    public bool TryUse(string name)
    {
        var response = TryLookUp(name);
        if (response is not null)
        {
            if (!_used.Contains(response.Variable))
                _used.Add(response.Variable);
            return true;
        }
        return false;
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

    public ImmutableArray<VariableSymbol> GetVariables() =>
        _variables.Values.ToImmutableArray();

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

    public ImmutableArray<VariableSymbol> GetAllUsed()
    {
        var used = _used.ToList();

        var blockChildren = Children.Where(c => c.Kind == ScopeKind.Block);
        foreach (var child in blockChildren)
        {
            used.AddRange(child.GetAllUsed());
        }

        return used.ToImmutableArray();
    }

    public ImmutableArray<VariableSymbol> GetOutsideDependencies()
    {
        var all = new [] { GetAllArguments(), GetAllBlockVariables() };
        var inner = all.SelectMany(x => x).ToList();
        var used = GetAllUsed();

        return used.Except(inner).ToImmutableArray();
    }
}


