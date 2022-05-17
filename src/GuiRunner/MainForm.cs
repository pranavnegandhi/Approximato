using Notadesigner.Pomodour.App.Properties;
using Notadesigner.Pomodour.Core;

namespace Notadesigner.Pomodour.App
{
    public partial class MainForm : Form
    {
        private readonly PomoEngine _engine;

        public MainForm(PomoEngine engine)
        {
            _ = engine ?? throw new ArgumentNullException(nameof(engine));

            _engine = engine;
            _engine.StateChange += EngineStateChangeHandler;
            _engine.Progress += EngineProgressHandler;

            InitializeComponent();
        }

        private void StartPomodoroClickHandler(object sender, EventArgs e)
        {
            _engine.MoveNext();
        }

        private void StartBreakClickHandler(object sender, EventArgs e)
        {
            _engine.MoveNext();
        }

        private void EngineProgressHandler(object? sender, ProgressEventArgs e)
        {
            Pomodoro1ProgressBar.Invoke(() =>
            {
                Pomodoro1ProgressBar.Value = Convert.ToInt32(e.ElapsedDuration.TotalSeconds);
                Pomodoro1ProgressBar.Maximum = Convert.ToInt32(e.TotalDuration.TotalSeconds);
                Pomodoro1ProgressBar.Text = $"{e.ElapsedDuration:mm\\:ss} / {e.TotalDuration:mm\\:ss}";
            });
        }

        private void EngineStateChangeHandler(object? sender, StateChangeEventArgs e)
        {
            if (e.State == EngineState.AppReady)
            {
                Pomodoro1ProgressBar.ProgressColor = SystemColors.Highlight;
            }

            if (e.State == EngineState.PomodoroCompleted)
            {
                if (GuiRunnerSettings.Default.AutoAdvance)
                {
                    _engine.MoveNext();
                }
                else
                {
                }

                Pomodoro1ProgressBar.ProgressColor = SystemColors.GradientActiveCaption;
            }

            if (e.State == EngineState.BreakCompleted)
            {
                if (GuiRunnerSettings.Default.AutoAdvance)
                {
                    _engine.MoveNext();
                }
                else
                {
                }

                Pomodoro1ProgressBar.ProgressColor = SystemColors.Highlight;
            }

            Invoke(() => Text = $"Round {e.RoundCounter + 1} of 4");
        }
    }
}