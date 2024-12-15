using Calculator.Core;
using Calculator.Core.Data;

namespace Calculator.Console;

public class ConsoleOutput : ICalculatorOutput
{
    public void OnCompleted()
    {
        var originalColor = System.Console.ForegroundColor;
        try
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Completed");
        }
        finally
        {
            System.Console.ForegroundColor = originalColor;
        }
    }

    public void OnError(Exception error)
    {
        System.Console.Error.WriteLine(error.Message);
        System.Console.Error.WriteLine(error.StackTrace);
    }

    public void OnNext(char value)
    {
        System.Console.Write(value);
    }
}