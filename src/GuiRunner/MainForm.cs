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
            circularProgressBar1.Invoke(() =>
            {
                circularProgressBar1.Value = Convert.ToInt32(e.ElapsedDuration.TotalSeconds);
#if WINDOWS7_0_OR_GREATER
                circularProgressBar1.Text = $"{e.ElapsedDuration:mm\\:ss} / {e.TotalDuration:mm\\:ss}";
#endif
            });
        }

        private void EngineStateChangeHandler(object? sender, StateChangeEventArgs e)
        {
            if (e.State == EngineState.AppReady)
            {
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
            }

            Invoke(() => Text = $"Round {e.RoundCounter + 1} of 4");
        }
    }
}