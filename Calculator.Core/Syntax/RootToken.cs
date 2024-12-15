namespace Calculator.Core.Syntax;

record RootToken : ICalculatorToken
{
    public override string ToString()
    {
        return "root";
    }
}