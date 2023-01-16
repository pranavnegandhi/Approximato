namespace Notadesigner.Tom.Core
{
    public sealed class PomoEngineSettings
    {
        public PomoEngineSettings(int maximumRounds, TimeSpan pomodoroDuration, TimeSpan shortBreakDuration, TimeSpan longBreakDuration)
        {
            MaximumRounds = maximumRounds;
            PomodoroDuration = pomodoroDuration;
            ShortBreakDuration = shortBreakDuration;
            LongBreakDuration = longBreakDuration;
        }

        public int MaximumRounds { get; set; }

        public TimeSpan PomodoroDuration { get; init; }

        public TimeSpan ShortBreakDuration { get; init; }

        public TimeSpan LongBreakDuration { get; init; }
    }
}