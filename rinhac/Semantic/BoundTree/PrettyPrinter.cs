namespace Rinha.Semantic.BoundTree;

public static class PrettyPrinter
{
    public static void Print(Node node) =>
        PrintNode(node);

    public static void Print(BoundScope scope) =>
        PrintScope(scope);

    private static void PrintNode(Node node, int identSize = 0)
    {
        var ident = String.Concat(Enumerable.Repeat(" ", identSize));
        Console.WriteLine(ident + node.ToString());

        var children = node.GetChildren();
        if (children is not null)
        {
            foreach (var child in children)
            {
                PrintNode(child, identSize + 2);
            }
        }
    }

    private static void PrintScope(BoundScope scope, int identSize = 0)
    {
        var ident = String.Concat(Enumerable.Repeat(" ", identSize));
        var childrenIdent = String.Concat(Enumerable.Repeat(" ", identSize + 2));

        Console.WriteLine(ident + scope.Kind.ToString());

        foreach (var variable in scope.GetVariables())
        {
            Console.WriteLine(childrenIdent + variable.Name);
        }

        foreach (var child in scope.Children)
        {
            PrintScope(child, identSize + 4);
        }
    }
}
