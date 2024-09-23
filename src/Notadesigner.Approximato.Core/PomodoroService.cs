using Microsoft.Extensions.Hosting;
using Notadesigner.Approximato.Messaging.Contracts;
using Stateless;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Core
{
    public class PomodoroService : BackgroundService, IEventHandler<UIEvent>
    {
        public event EventHandler<UIEvent>? EventReceived;

        private static readonly TimeSpan UnitIncrement = TimeSpan.FromSeconds(1);

        private readonly Func<PomodoroServiceSettings> _settingsFactory;

        private readonly StateMachine<TimerState, TimerTrigger> _stateMachine = new(TimerState.Begin);

        private readonly Channel<TransitionEvent> _transitionChannel;

        private readonly Channel<TimerEvent> _timerChannel;

        private TimeSpan _elapsedDuration = TimeSpan.Zero;

        private int _focusCounter = 0;

        private CancellationTokenSource? _activeCts;

        public PomodoroService(Func<PomodoroServiceSettings> settingsFactory, Channel<TransitionEvent> transitionChannel, Channel<TimerEvent> timerChannel)
        {
            _settingsFactory = settingsFactory;
            _transitionChannel = transitionChannel;
            _timerChannel = timerChannel;

            ConfigureStates(_stateMachine);
        }

        public async ValueTask HandleAsync(UIEvent @event, CancellationToken token = default) =>
            await _stateMachine.FireAsync(@event.Trigger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void ConfigureStates(StateMachine<TimerState, TimerTrigger> stateMachine)
        {
            stateMachine.OnUnhandledTrigger((state, trigger) => { });
            stateMachine.OnTransitioned(transition => { });

            stateMachine.Configure(TimerState.Abandoned)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Abandoned, _focusCounter));
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Begin)
                .OnEntry(() =>
                {
                    _focusCounter = 0;
                    _transitionChannel.Writer.TryWrite(new TransitionEvent(TimerState.Begin, _focusCounter));
                })
                .Permit(TimerTrigger.Focus, TimerState.Focused);

            stateMachine.Configure(TimerState.End)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.End, _focusCounter));
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Finished)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Finished, _focusCounter));

                    _activeCts = new();
                    var _ = RunFinishedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => _focusCounter < _settingsFactory().MaximumRounds)
                .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => _focusCounter >= _settingsFactory().MaximumRounds);

            stateMachine.Configure(TimerState.Focused)
                .OnEntryFromAsync(TimerTrigger.Focus, async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, ++_focusCounter));

                    _activeCts = new();
                    var _ = RunFocusedAsync(_activeCts.Token);
                })
                .OnEntryFromAsync(TimerTrigger.Resume, async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, _focusCounter)); /// Don't increment focusCounter when resuming an interrupted session

                    _activeCts = new();
                    var _ = RunFocusedAsync(_activeCts.Token);
                })
                .OnEntryFromAsync(TimerTrigger.Continue, async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, ++_focusCounter));

                    _activeCts = new();
                    var _ = RunFocusedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
                .Permit(TimerTrigger.Timeout, TimerState.Finished);

            stateMachine.Configure(TimerState.Interrupted)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Interrupted, _focusCounter));

                    _activeCts = new();
                    var _ = RunInterruptedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Resume, TimerState.Focused);

            stateMachine.Configure(TimerState.Refreshed)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Refreshed, _focusCounter));

                    _activeCts = new();
                    var _ = RunRefreshedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Continue, TimerState.Focused);

            stateMachine.Configure(TimerState.Relaxed)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Relaxed, _focusCounter));

                    _activeCts = new();
                    var _ = RunRelaxedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

            stateMachine.Configure(TimerState.Stopped)
                .OnEntryAsync(async () =>
                {
                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _activeCts?.Cancel();
                    _activeCts?.Dispose();
                    _activeCts = null;

                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Stopped, _focusCounter));

                    _activeCts = new();
                    var _ = RunStoppedAsync(_activeCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.End);
        }

        private async Task RunFocusedAsync(CancellationToken cancellationToken)
        {
            var delay = _settingsFactory().FocusDuration;
            var elapsed = TimeSpan.Zero;
            while (elapsed <= delay && !cancellationToken.IsCancellationRequested)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, delay));

                await Task.Delay(UnitIncrement, cancellationToken);
                elapsed = elapsed.Add(UnitIncrement);
            }

            if (elapsed >= delay)
            {
                _elapsedDuration = elapsed;
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
            }
        }

        private async Task RunInterruptedAsync(CancellationToken cancellationToken)
        {
            var elapsed = TimeSpan.Zero;
            while (!cancellationToken.IsCancellationRequested)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, TimeSpan.FromMinutes(59)));

                await Task.Delay(UnitIncrement, cancellationToken);
                elapsed = elapsed.Add(UnitIncrement);
            }
        }

        private async Task RunFinishedAsync(CancellationToken cancellationToken)
        {
            if (_settingsFactory().LenientMode)
            {
                var total = _settingsFactory().FocusDuration;
                var elapsed = _elapsedDuration;
                while (!cancellationToken.IsCancellationRequested)
                {
                    _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, total));

                    await Task.Delay(UnitIncrement, cancellationToken);
                    elapsed = elapsed.Add(UnitIncrement);
                }
            }
            else
            {
                await _stateMachine.FireAsync(TimerTrigger.Continue);
            }
        }

        private async Task RunRelaxedAsync(CancellationToken cancellationToken)
        {
            var delay = _settingsFactory().ShortBreakDuration;
            var elapsed = TimeSpan.Zero;
            while (elapsed <= delay && !cancellationToken.IsCancellationRequested)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, delay));

                await Task.Delay(UnitIncrement, cancellationToken);
                elapsed = elapsed.Add(UnitIncrement);
            }

            if (elapsed >= delay)
            {
                _elapsedDuration = elapsed;
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
            }
        }

        private async Task RunRefreshedAsync(CancellationToken cancellationToken)
        {
            await _stateMachine.FireAsync(TimerTrigger.Continue);
        }

        private async Task RunStoppedAsync(CancellationToken cancellationToken)
        {
            var delay = _settingsFactory().LongBreakDuration;
            var elapsed = TimeSpan.Zero;
            while (elapsed <= delay && !cancellationToken.IsCancellationRequested)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, delay));

                await Task.Delay(UnitIncrement, cancellationToken);
                elapsed = elapsed.Add(UnitIncrement);
            }

            if (elapsed >= delay)
            {
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
            }
        }
    }
}