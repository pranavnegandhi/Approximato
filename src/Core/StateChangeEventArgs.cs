namespace Notadesigner.Pomodour.Core
{
    public class StateChangeEventArgs : EventArgs
    {
        public StateChangeEventArgs(EngineState state, int roundCounter)
        {
            State = state;
            RoundCounter = roundCounter;
        }

        public EngineState State
        {
            get;
            init;
        }

        public int RoundCounter
        {
            get;
            init;
        }
    }
}