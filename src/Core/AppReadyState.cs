namespace Notadesigner.Tom.Core
{
    public class AppReadyState : IdleState
    {
        public AppReadyState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(AppReadyState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            _queue.Enqueue(new Notification(new WorkSessionState(0, _settings, _queue)));
        }

        public override EngineState State => EngineState.AppReady;
    }
}