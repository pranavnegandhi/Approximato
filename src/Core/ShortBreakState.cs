namespace Notadesigner.Tom.Core
{
    public class ShortBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration;

        public ShortBreakState(int roundCounter, Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
            : base(roundCounter, settingsFactory, queue)
        {
            StateDuration = settingsFactory().ShortBreakDuration;
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(ShortBreakState));
            Notification? notification = null;

            while (_timeElapsed < StateDuration)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    notification = new Notification(new AppReadyState(0, _settingsFactory, _queue));
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));
                OnProgress(_timeElapsed, StateDuration);
            }

            notification ??= new Notification(new BreakCompletedState(RoundCounter + 1, _settingsFactory, _queue));
            _queue.Enqueue(notification);
        }

        public override EngineState State => EngineState.ShortBreak;
    }
}