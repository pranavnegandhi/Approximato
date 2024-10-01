namespace Notadesigner.Approximato.Core
{
    public sealed class StateHostSettings(int maximumRounds,
        TimeSpan focusDuration,
        TimeSpan shortBreakDuration,
        TimeSpan longBreakDuration,
        bool lenientMode)
    {
        public TimeSpan FocusDuration { get; init; } = focusDuration;

        public bool LenientMode { get; init; } = lenientMode;

        public TimeSpan LongBreakDuration { get; init; } = longBreakDuration;

        public int MaximumRounds { get; set; } = maximumRounds;

        public TimeSpan ShortBreakDuration { get; init; } = shortBreakDuration;
    }
}