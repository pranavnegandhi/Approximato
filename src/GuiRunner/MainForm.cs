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

            InitializeComponent();
        }

        private void StartPomodoroClickHandler(object sender, EventArgs e)
        {
            StartPomodoro.Enabled = false;
            StartBreak.Enabled = false;
            _engine.MoveNext();
        }

        private void StartBreakClickHandler(object sender, EventArgs e)
        {
            StartPomodoro.Enabled = false;
            StartBreak.Enabled = false;
            _engine.MoveNext();
        }

        private void EngineStateChangeHandler(object sender, StateChangeEventArgs e)
        {
            if (e.State == EngineState.AppReady)
            {
                StartPomodoro.Invoke(() => StartPomodoro.Enabled = true);
            }

            if (e.State == EngineState.PomodoroCompleted)
            {
                if (GuiRunnerSettings.Default.AutoAdvance)
                {
                    _engine.MoveNext();
                }
                else
                {
                    StartBreak.Invoke(() => StartBreak.Enabled = true);
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
                    StartPomodoro.Invoke(() => StartPomodoro.Enabled = true);
                }
            }

            Invoke(() => Text = $"Round {e.RoundCounter + 1} of 4");
        }
    }
}