namespace Notadesigner.Approximato.Core
{
    public sealed class PomodoroServiceSettings
    {
        public PomodoroServiceSettings(int maximumRounds, TimeSpan focusDuration, TimeSpan shortBreakDuration, TimeSpan longBreakDuration, bool lenientMode)
        {
            MaximumRounds = maximumRounds;
            FocusDuration = focusDuration;
            ShortBreakDuration = shortBreakDuration;
            LongBreakDuration = longBreakDuration;
            LenientMode = lenientMode;
        }

        public TimeSpan FocusDuration { get; init; }

        public bool LenientMode { get; init; }

        public TimeSpan LongBreakDuration { get; init; }

        public int MaximumRounds { get; set; }

        public TimeSpan ShortBreakDuration { get; init; }
    }
}