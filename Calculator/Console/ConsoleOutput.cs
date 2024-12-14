using Calculator.Core;

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
        var originalColor = System.Console.ForegroundColor;
        try
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(error.Message);
            System.Console.WriteLine(error.StackTrace);
        }
        finally
        {
            System.Console.ForegroundColor = originalColor;
        }
    }

    public void OnNext(char value)
    {
        System.Console.Write(value);
    }
}