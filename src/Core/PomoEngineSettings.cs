namespace Notadesigner.Pomodour.Core
{
    public sealed class PomoEngineSettings
    {
        public PomoEngineSettings(TimeSpan pomodoroDuration, TimeSpan shortBreakDuration, TimeSpan longBreakDuration)
        {
            PomodoroDuration = pomodoroDuration;
            ShortBreakDuration = shortBreakDuration;
            LongBreakDuration = longBreakDuration;
        }

        public TimeSpan PomodoroDuration { get; init; }

        public TimeSpan ShortBreakDuration { get; init; }

        public TimeSpan LongBreakDuration { get; init; }
    }
}