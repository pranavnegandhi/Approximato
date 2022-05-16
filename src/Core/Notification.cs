namespace Notadesigner.Pomodour.Core
{
    public class Notification
    {
        public Notification(IEngineState state)
        {
            State = state;
        }

        public IEngineState State { get; private set; }
    }
}