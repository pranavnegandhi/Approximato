namespace Notadesigner.Tom.Core
{
    public class WorkCompletedState : IdleState
    {
        public WorkCompletedState(int roundCounter, Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
            : base(roundCounter, settingsFactory, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(WorkCompletedState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            var maximumRounds = _settingsFactory().MaximumRounds;
            IEngineState nextState = RoundCounter < maximumRounds ? new ShortBreakState(RoundCounter, _settingsFactory, _queue) : new LongBreakState(RoundCounter, _settingsFactory, _queue);
            _queue.Enqueue(new Notification(nextState));
        }

        public override EngineState State => EngineState.WorkCompleted;
    }
}