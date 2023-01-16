namespace Notadesigner.Tom.Core
{
    public class AppReadyState : IdleState
    {
        public AppReadyState(int roundCounter, Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
            : base(roundCounter, settingsFactory, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("Entering {stateMachine}", nameof(AppReadyState));

            await _moveNextSignal.WaitAsync(cancellationToken);

            _queue.Enqueue(new Notification(new WorkSessionState(0, _settingsFactory, _queue)));
        }

        public override EngineState State => EngineState.AppReady;
    }
}