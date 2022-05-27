using Notadesigner.Tom.App.Properties;
using Notadesigner.Tom.Core;

namespace Notadesigner.Tom.App
{
    public partial class MainForm : Form
    {
        private readonly PomoEngine _engine;

        private readonly CircularProgressBar.CircularProgressBar[] _allProgressBars = new CircularProgressBar.CircularProgressBar[]
        {
            ProgressBarFactory.Create(),
            ProgressBarFactory.Create(),
            ProgressBarFactory.Create(),
            ProgressBarFactory.Create()
        };

        private CircularProgressBar.CircularProgressBar _activeProgressBar;

        public MainForm(PomoEngine engine)
        {
            _ = engine ?? throw new ArgumentNullException(nameof(engine));

            _engine = engine;
            _engine.Progress += EngineProgressHandler;
            _engine.StateChange += EngineStateChangeHandler;
            _engine.StartSession += EngineStartSessionHandler;

            InitializeComponent();

            Icon = GuiRunnerResources.MainIcon;

            Array.ForEach(_allProgressBars, p => ProgressBarsContainer.Controls.Add(p));
            _activeProgressBar = _allProgressBars[0];

            VisibleChanged += (s, e) =>
            {
                if (Visible)
                {
                    var rect = Screen.PrimaryScreen.WorkingArea;
                    Location = new Point(rect.Right - Width, rect.Bottom - Height);
                }
            };
        }

        private void EngineStartSessionHandler(object? sender, EventArgs e)
        {
            Array.ForEach(_allProgressBars, p =>
            {
                p.ProgressColor = SystemColors.Highlight;
                p.Text = "__:__ / __:__";
                p.Value = 0;
            });
        }

        private void EngineProgressHandler(object? sender, ProgressEventArgs e)
        {
            if (!_activeProgressBar.IsHandleCreated)
            {
                return;
            }

            _activeProgressBar.Invoke(() =>
            {
                _activeProgressBar.Value = Convert.ToInt32(e.ElapsedDuration.TotalSeconds);
                _activeProgressBar.Maximum = Convert.ToInt32(e.TotalDuration.TotalSeconds);
                _activeProgressBar.Text = $"{e.ElapsedDuration:mm\\:ss} / {e.TotalDuration:mm\\:ss}";
            });
        }

        private void EngineStateChangeHandler(object? sender, StateChangeEventArgs e)
        {
            switch (e.State)
            {
                case EngineState.AppReady:
                    _activeProgressBar = _allProgressBars[e.RoundCounter]; /// Point back to the first progress bar
                    break;

                case EngineState.BreakCompleted:
                    _activeProgressBar = _allProgressBars[e.RoundCounter]; /// Point to the progress bar that matches the active round
                    break;

                case EngineState.LongBreak:
                case EngineState.ShortBreak:
                    _activeProgressBar.ProgressColor = SystemColors.GradientActiveCaption;
                    _activeProgressBar.Invoke(() => _activeProgressBar.Text = "00:00 / 00:00");
                    break;

                case EngineState.WorkSession:
                    _activeProgressBar.Invoke(() =>
                    {
                        _activeProgressBar.Text = "00:00 / 00:00";
                        _activeProgressBar.Refresh();
                    });
                    break;
            }
        }
    }
}