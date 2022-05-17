namespace Notadesigner.Pomodour.Core
{
    public class PomodoroState : ActiveState
    {
        private readonly TimeSpan StateDuration;

        public PomodoroState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
            : base(roundCounter, settings, queue)
        {
            StateDuration = settings.PomodoroDuration;
        }

        public override async Task EnterAsync(CancellationToken cancellationToken)
        {
            _log.Verbose("Entering {state}", nameof(PomodoroState));

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

            _queue.Enqueue(new Notification(new PomodoroCompletedState(RoundCounter, _settings, _queue)));
        }

        public override EngineState State => EngineState.Pomodoro;
    }
}