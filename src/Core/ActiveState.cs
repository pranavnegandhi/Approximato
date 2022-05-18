using Serilog;

namespace Notadesigner.Tommy.Core
{
    public abstract class ActiveState : IEngineState
    {
        public event EventHandler<ProgressEventArgs>? Progress;

        protected readonly ILogger _log = Log.ForContext<ActiveState>();

        protected readonly PomoEngineSettings _settings;

        protected readonly NotificationsQueue _queue;

        private readonly int _roundCounter;

        protected TimeSpan _timeElapsed = TimeSpan.FromSeconds(0);

        public ActiveState(int roundCounter, PomoEngineSettings settings, NotificationsQueue queue)
        {
            _settings = settings;
            _roundCounter = roundCounter;
            _queue = queue;
        }

        public abstract Task EnterAsync(CancellationToken cancellationToken);

        public void MoveNext()
        {
        }

        public int RoundCounter
        {
            get { return _roundCounter; }
        }

        public abstract EngineState State { get; }

        protected void OnProgress(TimeSpan elapsedDuration, TimeSpan totalDuration)
        {
            Progress?.Invoke(this, new ProgressEventArgs(elapsedDuration, totalDuration));
        }
    }
}