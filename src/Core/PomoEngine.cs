using Microsoft.Extensions.Hosting;
using Serilog;

namespace Notadesigner.Pomodour.Core
{
    public class PomoEngine : IHostedService
    {
        public event EventHandler<StateChangeEventArgs>? StateChange;

        private readonly ILogger _log = Log.ForContext<PomoEngine>();

        private readonly NotificationsQueue _queue;

        private IEngineState _engineState;

        private CancellationTokenSource? _cts;

        private Task? _execute;

        public PomoEngine(NotificationsQueue queue)
        {
            _queue = queue;
            _engineState = new AppReadyState(0, _queue);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Starting {serviceName}", nameof(PomoEngine));
            await Task.CompletedTask;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _execute = ExecuteAsync(_cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts?.Cancel();
            _ = _execute ?? throw new InvalidOperationException();

            await _execute;
        }

        public void MoveNext()
        {
            _engineState.MoveNext();
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Executing {serviceName}", nameof(PomoEngine));
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _engineState.EnterAsync(cancellationToken);
                    var notification = await _queue.DequeueAsync(cancellationToken);
                    _engineState = notification?.State ?? throw new InvalidOperationException();

                    StateChange?.Invoke(this, new StateChangeEventArgs(_engineState.State, _engineState.RoundCounter));
                }
                catch (TaskCanceledException)
                {
                    /// Nothing to do here
                }
                catch (Exception exception)
                {
                    _log.Fatal(exception, "A fatal exception occured in the {serviceName}", nameof(PomoEngine));
                }
            }
        }
    }
}