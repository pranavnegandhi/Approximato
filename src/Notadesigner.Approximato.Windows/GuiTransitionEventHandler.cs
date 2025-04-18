using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Stateless;

namespace Notadesigner.Approximato.Windows;

public class GuiTransitionEventHandler : IEventHandler<TransitionEvent>
{
    public event EventHandler<TransitionEvent>? EventReceived;

    private TimerState _timerState;

    private int _focusCounter;

    private readonly StateMachine<TimerState, TimerTrigger> _stateMachine = new(TimerState.Begin);

    internal event EventHandler<int>? Abandoned;

    internal event EventHandler<int>? Begin;

    internal event EventHandler<int>? End;

    internal event EventHandler<int>? Finished;

    internal event EventHandler<int>? Refreshed;

    internal event EventHandler<int>? FocusedEntry;

    internal event EventHandler? FocusedExit;

    internal event EventHandler<int>? Interrupted;

    internal event EventHandler<int>? Relaxed;

    internal event EventHandler<int>? Stopped;

    public GuiTransitionEventHandler()
    {
        _stateMachine = new StateMachine<TimerState, TimerTrigger>(TimerState.Begin);
        ConfigureStates(_stateMachine);
        _stateMachine.Fire(TimerTrigger.Reset);
    }

    async ValueTask IEventHandler<TransitionEvent>.HandleAsync(TransitionEvent @event, CancellationToken cancellationToken)
    {
        _timerState = @event.TimerState;
        _focusCounter = @event.FocusCounter;

        switch (@event.TimerState)
        {
            case TimerState.Abandoned:
                await _stateMachine.FireAsync(TimerTrigger.Abandon);
                break;

            case TimerState.Begin:
                await _stateMachine.FireAsync(TimerTrigger.Reset);
                break;

            case TimerState.End:
            case TimerState.Finished:
            case TimerState.Refreshed:
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
                break;

            case TimerState.Focused:
                if (_stateMachine.State == TimerState.Begin)
                {
                    await _stateMachine.FireAsync(TimerTrigger.Focus);
                }
                else if (_stateMachine.State == TimerState.Interrupted)
                {
                    await _stateMachine.FireAsync(TimerTrigger.Resume);
                }
                else if (_stateMachine.State == TimerState.Refreshed)
                {
                    await _stateMachine.FireAsync(TimerTrigger.Continue);
                }
                break;

            case TimerState.Interrupted:
                await _stateMachine.FireAsync(TimerTrigger.Interrupt);
                break;

            case TimerState.Relaxed:
            case TimerState.Stopped:
                await _stateMachine.FireAsync(TimerTrigger.Continue);
                break;
        }
    }

    private void ConfigureStates(StateMachine<TimerState, TimerTrigger> stateMachine)
    {
        stateMachine.OnUnhandledTrigger((state, trigger) => { });

        stateMachine.Configure(TimerState.Abandoned)
            .OnEntry(() => Abandoned?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Begin)
            .OnEntry(() => Begin?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Focus, TimerState.Focused)
            .PermitReentry(TimerTrigger.Reset); /// Explicitly allowed to easily set UI state on application startup

        stateMachine.Configure(TimerState.End)
            .OnEntry(() => End?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Finished)
            .OnEntry(() => Finished?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => _timerState == TimerState.Stopped)
            .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => _timerState == TimerState.Relaxed);

        stateMachine.Configure(TimerState.Focused)
            .OnEntry(() => FocusedEntry?.Invoke(this, _focusCounter))
            .OnExit(() => FocusedExit?.Invoke(this, EventArgs.Empty))
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
            .Permit(TimerTrigger.Timeout, TimerState.Finished);

        stateMachine.Configure(TimerState.Interrupted)
            .OnEntry(() => Interrupted?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Resume, TimerState.Focused);

        stateMachine.Configure(TimerState.Refreshed)
            .OnEntry(() => Refreshed?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Continue, TimerState.Focused);

        stateMachine.Configure(TimerState.Relaxed)
            .OnEntry(() => Relaxed?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

        stateMachine.Configure(TimerState.Stopped)
            .OnEntry(() => Stopped?.Invoke(this, _focusCounter))
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Timeout, TimerState.End);
    }
}