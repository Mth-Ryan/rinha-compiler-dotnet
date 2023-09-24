namespace Rinha.Semantic.BoundTree;

public static class PrettyPrinter
{
    public static void Print(Node node) =>
        PrintNode(node);

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
}
