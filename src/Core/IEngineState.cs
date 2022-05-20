namespace Notadesigner.Tom.Core
{
    public interface IEngineState
    {
        event EventHandler<ProgressEventArgs> Progress;

        Task EnterAsync(CancellationToken cancellationToken);

        void MoveNext();

        int RoundCounter { get; }

        EngineState State { get; }
    }
}