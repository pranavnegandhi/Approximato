using Serilog;

namespace Notadesigner.Pomodour.Core
{
    public abstract class ActiveState : IEngineState
    {
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
    }
}