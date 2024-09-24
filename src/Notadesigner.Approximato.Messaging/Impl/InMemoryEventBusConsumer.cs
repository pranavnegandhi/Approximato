using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Messaging.Contracts;
using Serilog;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.Impl
{
    internal sealed class InMemoryEventBusConsumer<T>(ChannelReader<Event<T>> bus, IServiceProvider serviceProvider) : IConsumer<T>
    {
        private readonly ChannelReader<Event<T>> _bus = bus;

        private readonly ILogger _logger = Log.ForContext<InMemoryEventBusConsumer<T>>();

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private CancellationTokenSource? _stoppingCts;

        public ValueTask DisposeAsync()
        {
            _stoppingCts?.Cancel();

            return ValueTask.CompletedTask;
        }

        public ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            EnsureStoppingTokenCreated();

            var handlers = _serviceProvider.GetServices<IEventHandler<T>>();
            var contextAccessor = _serviceProvider.GetRequiredService<IEventContextAccessor<T>>();

            if (!handlers.Any())
            {
                _logger.Debug("No handlers defined for {@EventType}", typeof(T).Name);

                return ValueTask.CompletedTask;
            }

            _ = Task.Run(async () =>
            {
                await StartProcessingAsync(handlers, contextAccessor).ConfigureAwait(false);
            }, _stoppingCts!.Token)
                .ConfigureAwait(false);

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

            _stoppingCts = new();
        }

        private async Task StartProcessingAsync(IEnumerable<IEventHandler<T>> handlers, IEventContextAccessor<T> contextAccessor)
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

                await Parallel.ForEachAsync(handlers,
                    _stoppingCts.Token,
                    async (handler, scopedToken) => await ExecuteHandlerAsync(handler, task, contextAccessor, scopedToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        private ValueTask ExecuteHandlerAsync(IEventHandler<T> handler, Event<T> task, IEventContextAccessor<T> contextAccessor, CancellationToken cancellationToken)
        {
            contextAccessor.Set(task); // set metadata and begin scope

            _ = Task.Run(async () => await handler.HandleAsync(task.Data, cancellationToken), cancellationToken)
                .ConfigureAwait(false);

            return ValueTask.CompletedTask;
        }
    }
}