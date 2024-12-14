using System.Globalization;

namespace Calculator.Core;

public record LiteralToken(decimal Value) : ICalculatorToken
{
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}