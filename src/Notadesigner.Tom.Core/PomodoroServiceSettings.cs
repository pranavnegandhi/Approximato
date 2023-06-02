namespace Notadesigner.Tom.Core
{
    public sealed class PomodoroServiceSettings
    {
        public PomodoroServiceSettings(int maximumRounds, TimeSpan pomodoroDuration, TimeSpan shortBreakDuration, TimeSpan longBreakDuration, bool autoAdvance)
        {
            MaximumRounds = maximumRounds;
            PomodoroDuration = pomodoroDuration;
            ShortBreakDuration = shortBreakDuration;
            LongBreakDuration = longBreakDuration;
            AutoAdvance = autoAdvance;
        }

        public int MaximumRounds { get; set; }

        public TimeSpan PomodoroDuration { get; init; }

        public TimeSpan ShortBreakDuration { get; init; }

        public TimeSpan LongBreakDuration { get; init; }

        public bool AutoAdvance { get; init; }
    }
}