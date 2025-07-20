using Notadesigner.Approximato.Messaging.Contracts;
using Serilog;
using Stateless;

namespace Notadesigner.Approximato.Core;

public class StateHost : IEventHandler<UIEvent>
{
    public event EventHandler<UIEvent>? EventReceived;

    private static readonly TimeSpan UnitIncrement = TimeSpan.FromSeconds(1);

    private readonly Func<StateHostSettings> _settingsFactory;

    private readonly StateMachine<TimerState, TimerTrigger> _stateMachine = new(TimerState.Begin);

    private readonly IProducer<TransitionEvent> _transitionproducer;

    private readonly IProducer<TimerEvent> _timerProducer;

    private TimeSpan _elapsedDuration = TimeSpan.Zero;

    private int _focusCounter = 0;

    private CancellationTokenSource? _activeCts;

    private readonly ILogger _logger = Log.ForContext<StateHost>();

    public StateHost(Func<StateHostSettings> settingsFactory, IProducer<TransitionEvent> transitionProducer, IProducer<TimerEvent> timerProducer)
    {
        _settingsFactory = settingsFactory;
        _transitionproducer = transitionProducer;
        _timerProducer = timerProducer;

        ConfigureStates(_stateMachine);
        _stateMachine.FireAsync(TimerTrigger.Reset);
    }

    public async ValueTask HandleAsync(UIEvent @event, CancellationToken token = default) =>
        await _stateMachine.FireAsync(@event.Trigger);

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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Abandoned, _focusCounter));
                await _transitionproducer.PublishAsync(@event);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
            })
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Begin)
            .OnEntryAsync(async () =>
            {
                _focusCounter = 0;
                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Begin, _focusCounter));
                await _transitionproducer.PublishAsync(@event);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
            })
            .Permit(TimerTrigger.Focus, TimerState.Focused)
            .PermitReentry(TimerTrigger.Reset); /// Explicitly allowed to easily set state on application startup

        stateMachine.Configure(TimerState.End)
            .OnEntryAsync(async () =>
            {
                /// Dispose the cancellationTokenSource
                /// as it is no longer needed
                _activeCts?.Cancel();
                _activeCts?.Dispose();
                _activeCts = null;

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.End, _focusCounter));
                await _transitionproducer.PublishAsync(@event);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
            })
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Finished)
            .OnEntryAsync(async () =>
            {
                /// Dispose the cancellationTokenSource
                /// as it is no longer needed
                _activeCts?.Dispose();
                _activeCts = null;

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Finished, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

                _activeCts = new();
                var _ = RunFinishedAsync(_activeCts.Token);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
            })
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => _focusCounter < _settingsFactory().MaximumRounds)
            .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => _focusCounter >= _settingsFactory().MaximumRounds);

        stateMachine.Configure(TimerState.Focused)
            .OnEntryFromAsync(TimerTrigger.Focus, async () =>
            {
                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Focused, ++_focusCounter));
                await _transitionproducer.PublishAsync(@event);

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

                /// Don't increment focusCounter when resuming an interrupted session
                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Focused, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Focused, ++_focusCounter));
                await _transitionproducer.PublishAsync(@event);

                _activeCts = new();
                var _ = RunFocusedAsync(_activeCts.Token);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Interrupted, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Refreshed, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

                _activeCts = new();
                await _stateMachine.FireAsync(TimerTrigger.Continue);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Relaxed, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

                _activeCts = new();
                var _ = RunRelaxedAsync(_activeCts.Token);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
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

                var @event = new Event<TransitionEvent>(new TransitionEvent(TimerState.Stopped, _focusCounter));
                await _transitionproducer.PublishAsync(@event);

                _activeCts = new();
                var _ = RunStoppedAsync(_activeCts.Token);
            })
            .OnExitAsync(transition =>
            {
                _logger.Debug("{Module} | {Source} -{Transition}-> {Destination} {Counter}",
                    nameof(StateHost),
                    transition.Source,
                    nameof(StateMachine<TimerState, TimerTrigger>.StateConfiguration.OnExitAsync),
                    transition.Destination,
                    _focusCounter);

                return Task.CompletedTask;
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
            var @event = new Event<TimerEvent>(new TimerEvent(elapsed, delay));
            await _timerProducer.PublishAsync(@event, cancellationToken);

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
            var @event = new Event<TimerEvent>(new TimerEvent(elapsed, TimeSpan.FromMinutes(59)));
            await _timerProducer.PublishAsync(@event, cancellationToken);

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
                var @event = new Event<TimerEvent>(new TimerEvent(elapsed, total));
                await _timerProducer.PublishAsync(@event, cancellationToken);

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
            var @event = new Event<TimerEvent>(new TimerEvent(elapsed, delay));
            await _timerProducer.PublishAsync(@event, cancellationToken);

            await Task.Delay(UnitIncrement, cancellationToken);
            elapsed = elapsed.Add(UnitIncrement);
        }

        if (elapsed >= delay)
        {
            _elapsedDuration = elapsed;
            await _stateMachine.FireAsync(TimerTrigger.Timeout);
        }
    }

    private async Task RunStoppedAsync(CancellationToken cancellationToken)
    {
        var delay = _settingsFactory().LongBreakDuration;
        var elapsed = TimeSpan.Zero;
        while (elapsed <= delay && !cancellationToken.IsCancellationRequested)
        {
            var @event = new Event<TimerEvent>(new TimerEvent(elapsed, delay));
            await _timerProducer.PublishAsync(@event, cancellationToken);

            await Task.Delay(UnitIncrement, cancellationToken);
            elapsed = elapsed.Add(UnitIncrement);
        }

        if (elapsed >= delay)
        {
            await _stateMachine.FireAsync(TimerTrigger.Timeout);
        }
    }
}