namespace Notadesigner.Tom.Core
{
    public readonly struct TransitionEvent
    {
        public TransitionEvent(TimerState timerState)
        {
            TimerState = timerState;
        }

        public TimerState TimerState
        {
            get;
            init;
        }

        public override string ToString() => $"{TimerState}";
    }
}