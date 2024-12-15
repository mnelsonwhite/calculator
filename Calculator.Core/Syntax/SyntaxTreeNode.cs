namespace Calculator.Core.Syntax;

public class SyntaxTreeNode(
    ICalculatorToken token,
    SyntaxTreeNode? left = null,
    SyntaxTreeNode? right = null)
{
    public readonly ICalculatorToken Token = token;
    public SyntaxTreeNode? Left = left;
    public SyntaxTreeNode? Right = right;

    public void RemoveChildren()
    {
        Left = null;
        Right = null;
    }

    public override string ToString()
    {
        return this.PrintTree();
    }
}