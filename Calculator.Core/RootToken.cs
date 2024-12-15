namespace Calculator.Core;

record RootToken : ICalculatorToken
{
    public override string ToString()
    {
        return "root";
    }
}