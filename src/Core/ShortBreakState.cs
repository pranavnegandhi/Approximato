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

            _queue.Enqueue(new Notification(new BreakCompletedState(RoundCounter + 1, _settingsFactory, _queue)));
        }

        public override EngineState State => EngineState.ShortBreak;
    }
}