namespace Notadesigner.Pomodour.Core
{
    public class LongBreakState : ActiveState
    {
        private readonly TimeSpan StateDuration = TimeSpan.FromSeconds(60);

        public LongBreakState(int roundCounter, NotificationsQueue queue)
            : base(roundCounter, queue)
        {
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(LongBreakState));

            while (_timeElapsed < StateDuration)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            _queue.Enqueue(new Notification(new AppReadyState(0, _queue)));
        }

        public override EngineState State => EngineState.LongBreak;
    }
}