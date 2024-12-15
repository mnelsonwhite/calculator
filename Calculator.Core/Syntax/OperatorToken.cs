namespace Calculator.Core.Syntax;

public record OperatorToken(OperatorType OperationType) : ICalculatorToken
{
    public override string? ToString()
    {
        return OperationType switch
        {
            OperatorType.Undefined => null,
            OperatorType.Addition => "+",
            OperatorType.Subtraction => "-",
            OperatorType.Multiplication => "x",
            OperatorType.Division => "/",
            OperatorType.Exponent => "^",
            OperatorType.Equals => "=",
            _ => null
        };
    }
}