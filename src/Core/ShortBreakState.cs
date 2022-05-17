namespace Notadesigner.Pomodour.Core
{
    public class ShortBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration;

        public ShortBreakState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
            StateDuration = settings.ShortBreakDuration;
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(ShortBreakState));

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

            _queue.Enqueue(new Notification(new BreakCompletedState(RoundCounter + 1, _settings, _queue)));
        }

        public override EngineState State => EngineState.ShortBreak;
    }
}