namespace Calculator.Utility;

internal class DelegateDisposable(Action callBack) : IDisposable
{
    public void Dispose()
    {
        callBack();
    }
}