using System.Globalization;

namespace Calculator.Core.Syntax;

public record LiteralToken(decimal Value) : ICalculatorToken
{
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}