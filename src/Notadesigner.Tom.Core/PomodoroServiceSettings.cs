namespace Notadesigner.Tom.Core
{
    public sealed class PomodoroServiceSettings
    {
        public PomodoroServiceSettings(int maximumRounds, TimeSpan pomodoroDuration, TimeSpan shortBreakDuration, TimeSpan longBreakDuration, bool lenientMode)
        {
            MaximumRounds = maximumRounds;
            PomodoroDuration = pomodoroDuration;
            ShortBreakDuration = shortBreakDuration;
            LongBreakDuration = longBreakDuration;
            LenientMode = lenientMode;
        }

        public bool LenientMode { get; init; }

        public TimeSpan LongBreakDuration { get; init; }

        public int MaximumRounds { get; set; }

        public TimeSpan PomodoroDuration { get; init; }

        public TimeSpan ShortBreakDuration { get; init; }
    }
}