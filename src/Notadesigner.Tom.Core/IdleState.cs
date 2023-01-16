using Serilog;

namespace Notadesigner.Tom.Core
{
    public abstract class IdleState : IEngineState
    {
        public event EventHandler<ProgressEventArgs>? Progress;

        protected readonly ILogger _logger = Log.ForContext<IdleState>();

        protected readonly Func<PomoEngineSettings> _settingsFactory;

        protected readonly NotificationsQueue _queue;

        protected readonly SemaphoreSlim _moveNextSignal = new(0);

        private readonly int _roundCounter;

        public IdleState(int roundCounter, Func<PomoEngineSettings> settingsFactory, NotificationsQueue queue)
        {
            _roundCounter = roundCounter;
            _settingsFactory = settingsFactory;
            _queue = queue;
        }

        public abstract Task EnterAsync(CancellationToken cancellationToken);

        public void MoveNext()
        {
            _moveNextSignal.Release();
        }

        public int RoundCounter
        {
            get { return _roundCounter; }
        }

        public abstract EngineState State { get; }
    }
}