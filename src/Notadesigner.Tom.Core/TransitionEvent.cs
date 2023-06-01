namespace Notadesigner.Tom.Core
{
    public readonly struct TransitionEvent
    {
        public TransitionEvent(TimerState timerState, int focusCounter)
        {
            FocusCounter = focusCounter;
            TimerState = timerState;
        }

        public int FocusCounter
        {
            get;
            init;
        }

        public TimerState TimerState
        {
            get;
            init;
        }

        public override string ToString() => $"{TimerState}, {FocusCounter}";
    }
}