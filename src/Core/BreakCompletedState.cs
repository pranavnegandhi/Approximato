namespace Notadesigner.Pomodour.Core
{
    public class BreakCompletedState : IdleState
    {
        public BreakCompletedState(int roundCounter, NotificationsQueue queue)
            : base(roundCounter, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(BreakCompletedState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            _queue.Enqueue(new Notification(new PomodoroState(RoundCounter, _queue)));
        }

        public override EngineState State => EngineState.BreakCompleted;
    }
}