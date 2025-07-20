using Notadesigner.Approximato.Messaging.Contracts;
using Serilog;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.Impl
{
    internal sealed class TransientBusEventConsumer<T>(ChannelReader<Event<T>> bus, IEnumerable<IEventHandler<T>>? handlers, IEventContextAccessor<T>? contextAccessor) : IConsumer<T>
    {
        private readonly ChannelReader<Event<T>> _bus = bus;

        private readonly ILogger _logger = Log.ForContext<TransientBusEventConsumer<T>>();

        private readonly IEnumerable<IEventHandler<T>> _handlers = handlers ?? [];

        private CancellationTokenSource? _stoppingCts;

        public ValueTask DisposeAsync()
        {
            _stoppingCts?.Cancel();

            return ValueTask.CompletedTask;
        }

        public ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            EnsureStoppingTokenCreated();

            if (!_handlers.Any())
            {
                _logger.Debug("No handlers defined for {@EventType}", typeof(T).Name);

                return ValueTask.CompletedTask;
            }

            _ = Task.Factory.StartNew(StartProcessingAsync, TaskCreationOptions.LongRunning).ConfigureAwait(false);

            return ValueTask.CompletedTask;
        }

        public async ValueTask StopAsync(CancellationToken cancellationToken = default)
        {
            await DisposeAsync().ConfigureAwait(false);
        }

        private void EnsureStoppingTokenCreated()
        {
            if (_stoppingCts is not null && !_stoppingCts.IsCancellationRequested)
            {
                _stoppingCts.Cancel();
            }

            _stoppingCts?.Dispose();
            _stoppingCts = new();
        }

        private async Task StartProcessingAsync()
        {
            var iterator = _bus.ReadAllAsync(_stoppingCts!.Token)
                .WithCancellation(_stoppingCts.Token)
                .ConfigureAwait(false);

            await foreach (var task in iterator)
            {
                if (_stoppingCts.IsCancellationRequested)
                {
                    break;
                }

                _logger.Debug("{Module} | Received {Event}",
                    nameof(TransientBusEventConsumer<T>),
                    task.Data?.ToString());

                await Parallel.ForEachAsync(_handlers,
                    _stoppingCts.Token,
                    async (handler, scopedToken) => await ExecuteHandlerAsync(handler, task, contextAccessor, scopedToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        private ValueTask ExecuteHandlerAsync(IEventHandler<T> handler, Event<T> task, IEventContextAccessor<T>? contextAccessor = default, CancellationToken cancellationToken = default)
        {
            _logger.Debug("{Module} | Dispatching {Event}",
                nameof(TransientBusEventConsumer<T>),
                task.Data?.ToString());

            contextAccessor?.Set(task); // set metadata and begin scope

            _ = Task.Run(async () => await handler.HandleAsync(task.Data, cancellationToken), cancellationToken)
                .ConfigureAwait(false);

            return ValueTask.CompletedTask;
        }
    }
}