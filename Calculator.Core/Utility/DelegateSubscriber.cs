namespace Calculator.Core.Utility;

internal class DelegateObserver<T>(
    Action? onCompleted = null,
    Action<Exception>? onError = null,
    Action<T>? onNext = null) : IObserver<T>
{
    public void OnCompleted() => onCompleted?.Invoke();
    public void OnError(Exception error) => onError?.Invoke(error);
    public void OnNext(T value) => onNext?.Invoke(value);
}

