using Notadesigner.Approximato.Messaging.Contracts;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.Impl
{
    internal sealed class InMemoryEventBusProducer<T>(ChannelWriter<Event<T>> bus) : IProducer<T>
    {
        private readonly ChannelWriter<Event<T>> _bus = bus;

        public ValueTask DisposeAsync()
        {
            _ = _bus.TryComplete();

            return ValueTask.CompletedTask;
        }

        public async ValueTask PublishAsync(Event<T> @event, CancellationToken cancellationToken = default)
        {
            await _bus.WriteAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}