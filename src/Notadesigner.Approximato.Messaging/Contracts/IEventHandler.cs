namespace Notadesigner.Approximato.Messaging.Contracts
{
    public interface IEventHandler<T>
    {
        event EventHandler<T>? EventReceived;

        ValueTask HandleAsync(T? @event, CancellationToken cancellationToken = default);
    }
}