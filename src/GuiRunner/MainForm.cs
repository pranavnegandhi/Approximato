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
            if (e.State == EngineState.AppReady || e.State == EngineState.BreakCompleted)
            {
                StartPomodoro.Invoke(() => StartPomodoro.Enabled = true);
            }

            if (e.State == EngineState.PomodoroCompleted)
            {
                StartBreak.Invoke(() => StartBreak.Enabled = true);
            }

            Invoke(() => Text = $"Round {e.RoundCounter + 1} of 4");
        }
    }
}