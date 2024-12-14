namespace Calculator.Core;

public interface ISyntaxTree
{
    void Add(LiteralToken token);
    void Add(OperatorToken token);
    void Add(OpenParenthesisToken token);
    bool Add(CloseParenthesisToken token);
    SyntaxTreeNode? Complete();
    void Reset();
}