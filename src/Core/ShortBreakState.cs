namespace Notadesigner.Pomodour.Core
{
    public class ShortBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration = TimeSpan.FromSeconds(15);

        public ShortBreakState(int roundCounter, NotificationsQueue queue)
            : base(roundCounter, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(ShortBreakState));

            while (_timeElapsed < StateDuration)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            _queue.Enqueue(new Notification(new BreakCompletedState(RoundCounter + 1, _queue)));
        }

        public override EngineState State => EngineState.ShortBreak;
    }
}