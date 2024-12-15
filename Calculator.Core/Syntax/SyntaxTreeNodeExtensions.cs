using System.Text;

namespace Calculator.Core.Syntax;

internal static class SyntaxTreeNodeExtensions
{
    public static string PrintTree(this SyntaxTreeNode? root)
    {
        if (root == null)
            return "Empty Tree";

        var stringBuilder = new StringBuilder();
        PrintNode(root, "", true, stringBuilder);
        return stringBuilder.ToString();
    }

    private static void PrintNode(SyntaxTreeNode node, string indent, bool isLast, StringBuilder stringBuilder)
    {
        // Print current node
        stringBuilder.Append(indent);

        if (isLast)
        {
            
            stringBuilder.Append("└── ");
            indent += "    ";
        }
        else
        {
            stringBuilder.Append("├── ");
            indent += "│   ";
        }
        
        stringBuilder.AppendLine(node.Token.ToString());

        // Recursively print child nodes
        var children = new List<SyntaxTreeNode?> { node.Left, node.Right }
            .Where(n => n != null)
            .ToList();

        for (int i = 0; i < children.Count; i++)
        {
            PrintNode(children[i]!, indent, i == children.Count - 1, stringBuilder);
        }
    }
}