namespace Notadesigner.Tom.Core
{
    public readonly struct UIEvent
    {
        public UIEvent(TimerTrigger trigger)
        {
            Trigger = trigger;
        }

        public TimerTrigger Trigger { get; init; }
    }
}