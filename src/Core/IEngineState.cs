namespace Notadesigner.Pomodour.Core
{
    public interface IEngineState
    {
        Task EnterAsync(CancellationToken cancellationToken);

        void MoveNext();

        int RoundCounter { get; }

        EngineState State { get; }
    }
}