namespace Notadesigner.Tommy.Core
{
    public class LongBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration;

        public LongBreakState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
            StateDuration = settings.LongBreakDuration;
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
                    return;
                }
            }

            _queue.Enqueue(new Notification(new AppReadyState(0, _settings, _queue)));
        }

        public override EngineState State => EngineState.LongBreak;
    }
}