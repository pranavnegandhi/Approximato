using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Stateless;

namespace Notadesigner.Approximato.Data;

public class DbTransitionEventHandler : IEventHandler<TransitionEvent>
{
    public event EventHandler<TransitionEvent>? EventReceived;

    private TimerState _timerState;

    private DateTime _startedAt;

    private readonly TransitionStorageService _transitionStorageService;

    private readonly StateMachine<TimerState, TimerTrigger> _stateMachine = new(TimerState.Begin);

    private readonly SemaphoreSlim _stateMachineSemaphore = new(1);

    public DbTransitionEventHandler(TransitionStorageService storageService)
    {
        _transitionStorageService = storageService;
        ConfigureStates(_stateMachine);
        _stateMachine.FireAsync(TimerTrigger.Reset);
    }

    public async ValueTask HandleAsync(TransitionEvent @event, CancellationToken cancellationToken = default)
    {
        await _stateMachineSemaphore.WaitAsync(cancellationToken);
        _timerState = @event.TimerState;

        switch (@event.TimerState)
        {
            case TimerState.Abandoned:
                await _stateMachine.FireAsync(TimerTrigger.Abandon);
                break;

            case TimerState.Begin:
                await _stateMachine.FireAsync(TimerTrigger.Reset);
                break;

            case TimerState.End:
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
                break;

            case TimerState.Finished:
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

            case TimerState.Refreshed:
                await _stateMachine.FireAsync(TimerTrigger.Timeout);
                break;

            case TimerState.Relaxed:
            case TimerState.Stopped:
                await _stateMachine.FireAsync(TimerTrigger.Continue);
                break;
        }
        _stateMachineSemaphore.Release();
    }

    private void ConfigureStates(StateMachine<TimerState, TimerTrigger> stateMachine)
    {
        stateMachine.OnUnhandledTrigger((state, trigger) => { });

        stateMachine.Configure(TimerState.Abandoned)
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Begin)
                .Permit(TimerTrigger.Focus, TimerState.Focused)
                .PermitReentry(TimerTrigger.Reset); /// Explicitly allowed to easily set database state on application startup

        stateMachine.Configure(TimerState.End)
            .Permit(TimerTrigger.Reset, TimerState.Begin);

        stateMachine.Configure(TimerState.Finished)
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => _timerState == TimerState.Stopped)
            .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => _timerState == TimerState.Relaxed);

        stateMachine.Configure(TimerState.Focused)
                .OnEntryFromAsync(TimerTrigger.Focus, async transition =>
                {
                    /// What to do when entering a focus from the Begin state?
                    _startedAt = DateTime.UtcNow;
                    await _transitionStorageService.AddPomodoroAsync(_startedAt, 0);
                })
                .OnEntryFromAsync(TimerTrigger.Continue, transition =>
                {
                    _startedAt = DateTime.UtcNow;

                    return Task.CompletedTask;
                })
                .OnExitAsync(async transition =>
                {
                    await _transitionStorageService.AddTransitionAsync(_startedAt, TimerState.Focused);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
                .Permit(TimerTrigger.Timeout, TimerState.Finished);

        stateMachine.Configure(TimerState.Interrupted)
            .Permit(TimerTrigger.Resume, TimerState.Focused);

        stateMachine.Configure(TimerState.Refreshed)
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Continue, TimerState.Focused);

        stateMachine.Configure(TimerState.Relaxed)
            .OnEntryFromAsync(TimerTrigger.Continue, transition =>
            {
                _startedAt = DateTime.UtcNow;

                return Task.CompletedTask;
            })
            .OnExitAsync(async transition =>
            {
                await _transitionStorageService.AddTransitionAsync(_startedAt, TimerState.Relaxed);
            })
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

        stateMachine.Configure(TimerState.Stopped)
            .OnEntryFromAsync(TimerTrigger.Continue, transition =>
            {
                _startedAt = DateTime.UtcNow;

                return Task.CompletedTask;
            })
            .OnExitAsync(async transition =>
            {
                await _transitionStorageService.AddTransitionAsync(_startedAt, TimerState.Stopped);
            })
            .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
            .Permit(TimerTrigger.Timeout, TimerState.End);
    }
}