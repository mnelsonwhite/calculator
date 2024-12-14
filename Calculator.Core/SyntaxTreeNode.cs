namespace Calculator.Core;

public class SyntaxTreeNode(ICalculatorToken token)
{
    public readonly ICalculatorToken Token = token;
    public SyntaxTreeNode? Left;
    public SyntaxTreeNode? Right;

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