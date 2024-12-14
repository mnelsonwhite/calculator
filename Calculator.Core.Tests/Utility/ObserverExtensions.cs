namespace Calculator.Core.Tests.Utility;

public static class ObserverExtensions
{
    public static void OnNext(this IObserver<char> observer, string value)
    {
        foreach (var c in value)
        {
            observer.OnNext(c);
        }
    }
}