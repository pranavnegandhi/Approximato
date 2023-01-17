using Microsoft.Extensions.Hosting;
using Serilog;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notadesigner.Tom.Core
{
    public class PomoEngine : BackgroundService, INotifyPropertyChanged
    {
        public event EventHandler<ProgressEventArgs>? Progress;

        public event EventHandler<StateChangeEventArgs>? StateChange;

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ILogger _log = Log.ForContext<PomoEngine>();

        private readonly Func<PomoEngineSettings> _settingsFactory;

        private readonly NotificationsQueue _queue;

        private IEngineState _engineState;

        private CancellationTokenSource _engineStateCts = new();

        private TimeSpan _elapsedDuration;

        private TimeSpan _totalDuration;

        public PomoEngine(Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
        {
            _settingsFactory = settingsFactory;
            _queue = queue;
            _engineState = new AppReadyState(0, _settingsFactory, _queue);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Starting {serviceName}", nameof(PomoEngine));

            return base.StartAsync(cancellationToken);
        }

        public async Task ResetAsync()
        {
            if (_engineState.State == EngineState.AppReady)
            {
                return;
            }

            _engineStateCts.Cancel();
            await Task.Delay(TimeSpan.FromSeconds(2)); /// Stupid hack to get around multi-threaded operations, I think
            _engineStateCts = new CancellationTokenSource();
        }

        public void MoveNext()
        {
            _engineState.MoveNext();
        }

        public EngineState State => _engineState.State;

        public TimeSpan ElapsedDuration
        {
            get => _elapsedDuration;

            set
            {
                if (value != _elapsedDuration)
                {
                    _elapsedDuration = value;

                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan TotalDuration
        {
            get => _totalDuration;

            set
            {
                if (value != _totalDuration)
                {
                    _totalDuration = value;

                    OnPropertyChanged();
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Executing {serviceName}", nameof(PomoEngine));
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _engineState.EnterAsync(_engineStateCts.Token);
                    var notification = await _queue.DequeueAsync(cancellationToken);
                    _engineState.Progress -= EngineStateProgressHandler; /// Stop listening for Progress events from the current state instance
                    _engineState = notification?.State ?? throw new InvalidOperationException();
                    _engineState.Progress += EngineStateProgressHandler; /// Start listening for Progress events from the new state instance

                    StateChange?.Invoke(this, new StateChangeEventArgs(_engineState.State, _engineState.RoundCounter));
                }
                catch (TaskCanceledException)
                {
                    /// Nothing to do here
                }
                catch (OperationCanceledException)
                {
                    /// Nothing to do here
                }
                catch (Exception exception)
                {
                    _log.Fatal(exception, "A fatal exception occured in the {serviceName}", nameof(PomoEngine));
                }
            }
        }

        private void EngineStateProgressHandler(object? sender, ProgressEventArgs e)
        {
            ElapsedDuration = e.ElapsedDuration;
            TotalDuration = e.TotalDuration;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}