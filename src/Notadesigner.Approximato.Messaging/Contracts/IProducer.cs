namespace Notadesigner.Approximato.Messaging.Contracts
{
    public interface IProducer<T> : IAsyncDisposable
    {
        ValueTask PublishAsync(Event<T> @event, CancellationToken cancellationToken = default);
    }
}