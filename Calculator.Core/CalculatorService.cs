using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Calculator.Core;

public class CalculatorService : IDisposable
{
    private readonly ICalculatorOutput _output;
    private readonly IDisposable _subscription;
    private readonly Stack<char> _inputBuffer;
    private readonly ISyntaxTree _syntaxTree;

    public CalculatorService(
        ICalculatorInput input,
        ICalculatorOutput output,
        ISyntaxTree syntaxTree)
    {
        _output = output;
        _subscription = input.Subscribe(
            new DelegateObserver<char>(
                onNext: ReceiveInput
            )
        );
        _syntaxTree = syntaxTree;

        _inputBuffer = new Stack<char>();
    }

    private void ReceiveInput(char input)
    {
        if (TryParseOperator(input, out var operatorType) && _inputBuffer.Any())
        {
            ReceiveNumber(GetNumberFromBuffer());
            ReceiveOperator(operatorType.Value);
        }
        else if (char.IsDigit(input))
        {   
            _inputBuffer.Push(input);
            _output.OnNext(input);
        }
        else if (input == '(')
        {
            ReceiveOpenParenthesis();
        }
        else if (input == ')')
        {
            ReceiveCloseParenthesis();
        }
        else if (input == '.' && !_inputBuffer.Contains('.'))
        {
            if (_inputBuffer.Count == 0)
            {
                _inputBuffer.Push('0');
                _output.OnNext(input);
            }
            _inputBuffer.Push(input);
            _output.OnNext(input);
        }
    }

    private void ReceiveOpenParenthesis()
    {
        _syntaxTree.Add(new OpenParenthesisToken());
        Write("( ");
    }

    private void ReceiveCloseParenthesis()
    {
        if (_syntaxTree.Add(new CloseParenthesisToken()))
        {
            Write(" )");
        }
    }

    private decimal GetNumberFromBuffer()
    {
        var aggValue = new string(_inputBuffer.Reverse().ToArray());
        _inputBuffer.Clear();
        
        return decimal.TryParse(aggValue, out var value)
            ? value
            : throw new Exception("Unable to parse number from buffer");
    }
    
    private void ReceiveNumber(decimal number)
    {
        _syntaxTree.Add(new LiteralToken(number));
    }

    private void ReceiveOperator(OperatorType operatorType)
    {
        if (operatorType != OperatorType.Equals)
        {
            _syntaxTree.Add(new OperatorToken(operatorType));
        }

        _output.OnNext(' ');
        _output.OnNext(OperatorToChar(operatorType));
        _output.OnNext(' ');
        
        if (operatorType == OperatorType.Equals)
        {
            var rootNode = _syntaxTree.Complete();

            if (rootNode is not null)
            {
                OnComplete(rootNode);
            }
        }
    }
    
    private char OperatorToChar(OperatorType operatorType) => operatorType switch
    {
        OperatorType.Addition => '+',
        OperatorType.Subtraction => '-',
        OperatorType.Multiplication => 'x',
        OperatorType.Division => '/',
        OperatorType.Exponent => '^',
        OperatorType.Equals => '=',
        _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
    }; 

    private void OnComplete(SyntaxTreeNode rootNode)
    {
        var result = EvaluateNode(rootNode);
        WriteLine(result.ToString(CultureInfo.CurrentCulture));
        _syntaxTree.Reset();
    }

    private static decimal EvaluateNode(SyntaxTreeNode node)
    {
        return node.Token switch
        {
            LiteralToken literal => literal.Value,
            OperatorToken op => EvaluateOperator(
                op.OperationType,
                (new []{node.Left, node.Right})
                    .Where(n => n is not null)
                    .Cast<SyntaxTreeNode>()
                    .ToList()
            ),
            _ => 0
        };
    }

    private static decimal EvaluateOperator(OperatorType type, List<SyntaxTreeNode> nodes)
    {
        if (nodes.Count != 2)
        {
            return 0;
        }

        var left = EvaluateNode(nodes[0]);
        var right = EvaluateNode(nodes[1]);

        return type switch
        {
            OperatorType.Addition => left + right,
            OperatorType.Subtraction => left - right,
            OperatorType.Multiplication => left * right,
            OperatorType.Division => right != 0 ? left / right : 0,
            OperatorType.Exponent => (decimal)Math.Pow((double)left, (double)right),
            _ => 0
        };
    }

    void Write(string output)
    {
        foreach (var ch in output)
        {
            _output.OnNext(ch);
        }
    }

    void WriteLine(string output)
    {
        Write(output);
        Write(Environment.NewLine);
    }
    
    private static bool TryParseOperator(
        char value,
        [NotNullWhen(returnValue: true)]
        out OperatorType? operatorType)
    {
        operatorType = value switch
        {
            '+' => OperatorType.Addition,
            '-' => OperatorType.Subtraction,
            '*' or 'x' => OperatorType.Multiplication,
            '/' => OperatorType.Division,
            '^' => OperatorType.Exponent,
            '=' => OperatorType.Equals,
            _ => null
        };

        return operatorType is not null;
    }

    public void Dispose()
    {
        _output.OnCompleted();
        _subscription.Dispose();
    }
}