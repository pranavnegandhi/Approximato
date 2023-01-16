namespace Notadesigner.Tom.Core
{
    public class LongBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration;

        public LongBreakState(int roundCounter, Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
            : base(roundCounter, settingsFactory, queue)
        {
            StateDuration = settingsFactory().LongBreakDuration;
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(LongBreakState));

            while (_timeElapsed < StateDuration)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));
                OnProgress(_timeElapsed, StateDuration);

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            _queue.Enqueue(new Notification(new AppReadyState(0, _settingsFactory, _queue)));
        }

        public override EngineState State => EngineState.LongBreak;
    }
}