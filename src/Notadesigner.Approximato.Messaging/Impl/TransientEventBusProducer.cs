using Notadesigner.Approximato.Messaging.Contracts;
using Serilog;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.Impl
{
    internal sealed class TransientBusEventProducer<T>(ChannelWriter<Event<T>> bus) : IProducer<T>
    {
        private readonly ChannelWriter<Event<T>> _bus = bus;

        private readonly ILogger _logger = Log.ForContext<TransientBusEventProducer<T>>();

        public ValueTask DisposeAsync()
        {
            _ = _bus.TryComplete();

            return ValueTask.CompletedTask;
        }

        public async ValueTask PublishAsync(Event<T> @event, CancellationToken cancellationToken = default)
        {
            _logger.Debug("{Module} | Published {@Event}",
                nameof(TransientBusEventProducer<T>),
                @event.Data,
                @event.GetType());

            await _bus.WriteAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}