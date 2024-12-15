namespace Calculator.Core;

public interface ISyntaxTree
{
    bool Add(ICalculatorToken token);
    SyntaxTreeNode? Complete();
    void Reset();
}