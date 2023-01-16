namespace Notadesigner.Tom.Core
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(TimeSpan elapsedDuration, TimeSpan totalDuration)
        {
            ElapsedDuration = elapsedDuration;
            TotalDuration = totalDuration;
        }

        public TimeSpan ElapsedDuration
        {
            get;
            init;
        }

        public TimeSpan TotalDuration
        {
            get;
            init;
        }
    }
}