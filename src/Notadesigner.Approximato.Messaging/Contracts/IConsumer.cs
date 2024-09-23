namespace Notadesigner.Approximato.Messaging.Contracts
{
    public interface IConsumer : IAsyncDisposable
    {
        ValueTask StartAsync(CancellationToken cancellationToken = default);

        ValueTask StopAsync(CancellationToken cancellationToken = default);
    }

    public interface IConsumer<T> : IConsumer
    {
    }
}