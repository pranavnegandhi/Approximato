using Microsoft.Extensions.Hosting;
using Stateless;
using System.Threading.Channels;

namespace Notadesigner.Tom.Core
{
    public class PomodoroService : BackgroundService
    {
        private static readonly TimeSpan UnitIncrement = TimeSpan.FromSeconds(1);

        private readonly Func<PomodoroServiceSettings> _settingsFactory;

        private readonly StateMachine<TimerState, TimerTrigger> _stateMachine = new(TimerState.Begin);

        private readonly Channel<TransitionEvent> _transitionChannel;

        private readonly Channel<TimerEvent> _timerChannel;

        private readonly Channel<UIEvent> _serviceChannel;

        private int _focusCounter = 0;

        private CancellationTokenSource? _focusedCts;

        private CancellationTokenSource? _finishedCts;

        private CancellationTokenSource? _relaxedCts;

        private CancellationTokenSource? _refreshedCts;

        private CancellationTokenSource? _stoppedCts;

        public PomodoroService(Func<PomodoroServiceSettings> settingsFactory, Channel<TransitionEvent> transitionChannel, Channel<TimerEvent> timerChannel, Channel<UIEvent> serviceChannel)
        {
            _settingsFactory = settingsFactory;
            _transitionChannel = transitionChannel;
            _timerChannel = timerChannel;
            _serviceChannel = serviceChannel;

            ConfigureStates(_stateMachine);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _serviceChannel.Reader.WaitToReadAsync(stoppingToken))
            {
                stoppingToken.ThrowIfCancellationRequested();

                var @event = await _serviceChannel.Reader.ReadAsync();
                await _stateMachine.FireAsync(@event.Trigger);
            }
        }

        private void ConfigureStates(StateMachine<TimerState, TimerTrigger> stateMachine)
        {
            stateMachine.OnUnhandledTrigger((state, trigger) => { });
            stateMachine.OnTransitioned(transition => { });

            stateMachine.Configure(TimerState.Abandoned)
                .OnEntryAsync(async () =>
                {
                    _focusedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Abandoned, _focusCounter));

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _focusedCts?.Dispose();
                    _focusedCts = null;
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
                    _stoppedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.End, _focusCounter));

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _stoppedCts?.Dispose();
                    _stoppedCts = null;
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Finished)
                .OnEntryAsync(async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Finished, _focusCounter));

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _focusedCts?.Dispose();
                    _focusedCts = null;

                    _finishedCts = new();
                    var _ = RunFinishedAsync(_finishedCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => _focusCounter < _settingsFactory().MaximumRounds)
                .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => _focusCounter >= _settingsFactory().MaximumRounds);

            stateMachine.Configure(TimerState.Focused)
                .OnEntryFromAsync(TimerTrigger.Focus, async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, ++_focusCounter));

                    _focusedCts = new();
                    var _ = RunFocusedAsync(_focusedCts.Token);
                })
                .OnEntryFromAsync(TimerTrigger.Resume, async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, _focusCounter)); /// Don't increment focusCounter when resuming an interrupted session

                    _focusedCts = new();
                    var _ = RunFocusedAsync(_focusedCts.Token);
                })
                .OnEntryFromAsync(TimerTrigger.Continue, async () =>
                {
                    _refreshedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, ++_focusCounter));

                    _focusedCts = new();
                    var _ = RunFocusedAsync(_focusedCts.Token);

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _refreshedCts?.Dispose();
                    _refreshedCts = null;
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
                .Permit(TimerTrigger.Timeout, TimerState.Finished);

            stateMachine.Configure(TimerState.Interrupted)
                .OnEntryAsync(async () =>
                {
                    _focusedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Interrupted, _focusCounter));

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _focusedCts?.Dispose();
                    _focusedCts = null;
                })
                .Permit(TimerTrigger.Resume, TimerState.Focused);

            stateMachine.Configure(TimerState.Refreshed)
                .OnEntryAsync(async () =>
                {
                    _relaxedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Refreshed, _focusCounter));

                    _refreshedCts = new();
                    var _ = RunRefreshedAsync(_refreshedCts.Token);

                    _relaxedCts?.Dispose();
                    _relaxedCts = null;
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Continue, TimerState.Focused);

            stateMachine.Configure(TimerState.Relaxed)
                .OnEntryAsync(async () =>
                {
                    _finishedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Relaxed, _focusCounter));

                    _relaxedCts = new();
                    var _ = RunRelaxedAsync(_relaxedCts.Token);

                    _finishedCts?.Dispose();
                    _finishedCts = null;
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

            stateMachine.Configure(TimerState.Stopped)
                .OnEntryAsync(async () =>
                {
                    _finishedCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Stopped, _focusCounter));

                    _stoppedCts = new();
                    var _ = RunStoppedAsync(_stoppedCts.Token);

                    _finishedCts?.Dispose();
                    _finishedCts = null;
                })
                .Permit(TimerTrigger.Focus, TimerState.Focused)
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
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
            }
        }

        private async Task RunFinishedAsync(CancellationToken cancellationToken)
        {
            if (_settingsFactory().LenientMode)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UnitIncrement, cancellationToken);
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
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
            }
        }

        private async Task RunRefreshedAsync(CancellationToken cancellationToken)
        {
            if (_settingsFactory().LenientMode)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UnitIncrement, cancellationToken);
                }
            }
            else
            {
                await _stateMachine.FireAsync(TimerTrigger.Continue);
            }
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