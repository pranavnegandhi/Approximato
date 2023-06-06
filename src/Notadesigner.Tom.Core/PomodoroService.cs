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

        private CancellationTokenSource? _focusCts;

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

        private void ConfigureStates(StateMachine<TimerState, TimerTrigger> tomo)
        {
            tomo.OnUnhandledTrigger((state, trigger) => { });
            tomo.OnTransitioned(transition => { });

            tomo.Configure(TimerState.Abandoned)
                .OnEntry(() => _transitionChannel.Writer.TryWrite(new TransitionEvent(TimerState.Abandoned, _focusCounter)));

            tomo.Configure(TimerState.Begin)
                .OnEntry(() =>
                {
                    _focusCounter = 0;
                    _transitionChannel.Writer.TryWrite(new TransitionEvent(TimerState.Begin, _focusCounter));
                })
                .Permit(TimerTrigger.Focus, TimerState.Focused);

            tomo.Configure(TimerState.End)
                .OnEntry(() => _transitionChannel.Writer.TryWrite(new TransitionEvent(TimerState.End, _focusCounter)))
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            tomo.Configure(TimerState.Finished)
                .OnEntryAsync(EnterFinishedAsync)
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .PermitIf(TimerTrigger.Relax, TimerState.Relaxed, () => _focusCounter < _settingsFactory().MaximumRounds)
                .PermitIf(TimerTrigger.Stop, TimerState.Stopped, () => _focusCounter >= _settingsFactory().MaximumRounds);

            tomo.Configure(TimerState.Focused)
                .OnEntryFromAsync(TimerTrigger.Focus, async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, ++_focusCounter));

                    _focusCts = new();
                    var _ = RunFocusedAsync(_focusCts.Token);
                })
                .OnEntryFromAsync(TimerTrigger.Resume, async () =>
                {
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Focused, _focusCounter)); /// Don't increment focusCounter when resuming an interrupted session

                    _focusCts = new();
                    var _ = RunFocusedAsync(_focusCts.Token);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
                .Permit(TimerTrigger.Timeout, TimerState.Finished);

            tomo.Configure(TimerState.Interrupted)
                .OnEntryAsync(async () =>
                {
                    _focusCts?.Cancel();
                    await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Interrupted, _focusCounter));

                    /// Dispose the cancellationTokenSource
                    /// as it is no longer needed
                    _focusCts?.Dispose();
                })
                .Permit(TimerTrigger.Resume, TimerState.Focused);

            tomo.Configure(TimerState.Refreshed)
                .OnEntryAsync(EnterRefreshedAsync)
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Focus, TimerState.Focused);

            tomo.Configure(TimerState.Relaxed)
                .OnEntryAsync(EnterRelaxedAsync)
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

            tomo.Configure(TimerState.Stopped)
                .OnEntryAsync(EnterStoppedAsync)
                .Permit(TimerTrigger.Focus, TimerState.Focused)
                .Permit(TimerTrigger.Timeout, TimerState.End);
        }

        private async Task EnterFinishedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Finished, _focusCounter));

            if (!_settingsFactory().LenientMode)
            {
                /// TODO: Wait until signal to proceed is not received from the UI thread
                await _serviceChannel.Reader.WaitToReadAsync();
                await _serviceChannel.Reader.ReadAsync();
            }

            /// Dispose the cancellationTokenSource
            /// as it is no longer needed
            _focusCts?.Dispose();

            var trigger = TimerTrigger.Relax;
            if (_focusCounter >= _settingsFactory().MaximumRounds)
            {
                trigger = TimerTrigger.Stop;
            }

            await _stateMachine.FireAsync(trigger);
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
                _stateMachine.Fire(TimerTrigger.Timeout);
            }
        }

        private async Task EnterRefreshedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Refreshed, _focusCounter));

            if (!_settingsFactory().LenientMode)
            {
                /// TODO: Wait until signal to proceed is not received from the UI thread
                await _serviceChannel.Reader.WaitToReadAsync();
                var @event = await _serviceChannel.Reader.ReadAsync();
            }

            _stateMachine.Fire(TimerTrigger.Focus);
        }

        private async Task EnterRelaxedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Relaxed, _focusCounter));

            var delay = _settingsFactory().ShortBreakDuration;
            var elapsed = TimeSpan.Zero;
            while (elapsed <= delay)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, delay));

                await Task.Delay(UnitIncrement);
                elapsed = elapsed.Add(UnitIncrement);
            }

            _stateMachine.Fire(TimerTrigger.Timeout);
        }

        private async Task EnterStoppedAsync()
        {
            await _transitionChannel.Writer.WriteAsync(new TransitionEvent(TimerState.Stopped, _focusCounter));

            var delay = _settingsFactory().LongBreakDuration;
            var elapsed = TimeSpan.Zero;
            while (elapsed <= delay)
            {
                _timerChannel.Writer.TryWrite(new TimerEvent(elapsed, delay));

                await Task.Delay(UnitIncrement);
                elapsed = elapsed.Add(UnitIncrement);
            }

            _stateMachine.Fire(TimerTrigger.Timeout);
        }
    }
}