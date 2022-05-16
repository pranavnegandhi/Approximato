namespace Notadesigner.Pomodour.Core
{
    public class PomodoroCompletedState : IdleState
    {
        public PomodoroCompletedState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(PomodoroCompletedState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            IEngineState nextState = RoundCounter < 3 ? new ShortBreakState(RoundCounter, _settings, _queue) : new LongBreakState(RoundCounter, _settings, _queue);
            _queue.Enqueue(new Notification(nextState));
        }

        public override EngineState State => EngineState.PomodoroCompleted;
    }
}