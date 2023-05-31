namespace Notadesigner.Tom.Core
{
    public readonly struct TimerEvent
    {
        public TimerEvent(TimeSpan elapsed, TimeSpan total)
        {
            Elapsed = elapsed;
            TotalDuration = total;
        }

        public TimeSpan Elapsed
        {
            get;
            init;
        }

        public TimeSpan TotalDuration
        {
            get;
            init;
        }

        public override string ToString() => $"{TotalDuration}, {Elapsed}";
    }
}