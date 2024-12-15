using Calculator.Core;
using Calculator.Core.Data;
using Calculator.Utility;

namespace Calculator.Console;

public class ConsoleInput: ICalculatorInput, IDisposable
{
    private readonly List<IObserver<char>> _subscribers;
    private readonly Lazy<(Task Task, CancellationTokenSource Cancellation)> _start;
    public (Task Task, CancellationTokenSource Cancellation) Start() => _start.Value;

    public ConsoleInput()
    {
        _subscribers = [];
        _start = new(StartProcess);
    }

    private (Task, CancellationTokenSource) StartProcess()
    {
        var cancellation = new CancellationTokenSource();
        var task = Task.Run(() => InputProcess(cancellation), cancellation.Token);
        return (task, cancellation);
    }

    private void InputProcess(CancellationTokenSource cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            var keyInfo = System.Console.ReadKey(true);
            
            if (keyInfo.Key == ConsoleKey.C
                && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)
                || keyInfo.Key == ConsoleKey.Escape)
            {
                cancellation.Cancel();
            }
            
            foreach(var subscriber in _subscribers)
            {
                subscriber.OnNext(keyInfo.KeyChar);
            }
        }
    }
    
    public IDisposable Subscribe(IObserver<char> observer)
    {
        _subscribers.Add(observer);

        return new DelegateDisposable(
            () => _subscribers.Remove(observer)
        );
    }

    public void Dispose()
    {
        if (_start.IsValueCreated)
        {
            _start.Value.Cancellation.Cancel();
        }
        
        foreach (var subscriber in _subscribers)
        {
            subscriber.OnCompleted();
        }
    }
}