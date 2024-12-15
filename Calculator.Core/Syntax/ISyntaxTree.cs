namespace Calculator.Core.Syntax;

public interface ISyntaxTree
{
    bool Add(ICalculatorToken token);
    SyntaxTreeNode? Complete();
    void Reset();
}