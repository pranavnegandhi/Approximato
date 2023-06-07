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

        private CancellationTokenSource? _relaxedCts;

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
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Finished)
                .OnEntryAsync(EnterFinishedAsync)
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
                })
                .Permit(TimerTrigger.Resume, TimerState.Focused);

            stateMachine.Configure(TimerState.Refreshed)
                .OnEntryAsync(EnterRefreshedAsync)
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Focus, TimerState.Focused);

            stateMachine.Configure(TimerState.Relaxed)
                .OnEntryAsync(async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Relaxed, _focusCounter));

                    _relaxedCts = new();
                    var _ = RunRelaxedAsync(_relaxedCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

            stateMachine.Configure(TimerState.Stopped)
                .OnEntryAsync(async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Stopped, _focusCounter));

                    _stoppedCts = new();
                    var _ = RunStoppedAsync(_stoppedCts.Token);
                })
                .Permit(TimerTrigger.Focus, TimerState.Focused)
                .Permit(TimerTrigger.Timeout, TimerState.End);
        }

        private async Task EnterFinishedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Finished, _focusCounter));

            /// Dispose the cancellationTokenSource
            /// as it is no longer needed
            _focusedCts?.Dispose();

            await _stateMachine.FireAsync(TimerTrigger.Continue);
        }

        private async Task EnterRefreshedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Refreshed, _focusCounter));

            /// Dispose the cancellationTokenSource
            /// as it is no longer needed
            _relaxedCts?.Dispose();

            await _stateMachine.FireAsync(TimerTrigger.Focus);
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