namespace Notadesigner.Pomodour.Core
{
    public class BreakCompletedState : IdleState
    {
        public BreakCompletedState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(BreakCompletedState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            _queue.Enqueue(new Notification(new WorkSessionState(RoundCounter, _settings, _queue)));
        }

        public override EngineState State => EngineState.BreakCompleted;
    }
}