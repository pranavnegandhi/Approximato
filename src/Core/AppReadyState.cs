namespace Notadesigner.Pomodour.Core
{
    public class AppReadyState : IdleState
    {
        public AppReadyState(int roundCounter, NotificationsQueue queue)
            : base(roundCounter, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(AppReadyState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            _queue.Enqueue(new Notification(new PomodoroState(0, _queue)));
        }

        public override EngineState State => EngineState.AppReady;
    }
}