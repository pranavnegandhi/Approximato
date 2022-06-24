using Notadesigner.Tom.App.Properties;
using Notadesigner.Tom.Core;

namespace Notadesigner.Tom.App
{
    public partial class MainForm : Form
    {
        private readonly PomoEngine _engine;

        private readonly CircularProgressBar.CircularProgressBar _workProgressBar = ProgressBarFactory.Create(SystemColors.Highlight, new Size(120, 120));

        private readonly CircularProgressBar.CircularProgressBar _breakProgressBar = ProgressBarFactory.Create(SystemColors.GradientActiveCaption, new Size(100, 100));

        private CircularProgressBar.CircularProgressBar _activeProgressBar;

        public MainForm(PomoEngine engine)
        {
            _ = engine ?? throw new ArgumentNullException(nameof(engine));

            _engine = engine;
            _engine.Progress += EngineProgressHandler;
            _engine.StateChange += EngineStateChangeHandler;

            InitializeComponent();

            Icon = GuiRunnerResources.MainIcon;

            progressBarsContainer.Controls.Add(_workProgressBar);
            progressBarsContainer.Controls.Add(_breakProgressBar);

            VisibleChanged += (s, e) =>
            {
                if (Visible)
                {
                    var rect = Screen.PrimaryScreen.WorkingArea;
                    Location = new Point(rect.Right - Width, rect.Bottom - Height);
                }
            };
        }

        private void EngineProgressHandler(object? sender, ProgressEventArgs e)
        {
            if (!_activeProgressBar.IsHandleCreated)
            {
                return;
            }

            Invoke(() =>
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
                case EngineState.LongBreak:
                case EngineState.ShortBreak:
                    _activeProgressBar = _breakProgressBar;
                    if (IsHandleCreated)
                    {
                        Invoke(() =>
                        {
                            _breakProgressBar.Text = "00:00 / 00:00";
                        });
                    }
                    break;

                case EngineState.WorkSession:
                    _activeProgressBar = _workProgressBar;
                    if (IsHandleCreated)
                    {
                        Invoke(() =>
                        {
                            _workProgressBar.Text = "__:__ / __:__";
                            _workProgressBar.Value = 0;

                            _breakProgressBar.Text = "__:__ / __:__";
                            _breakProgressBar.Value = 0;

                            currentPhaseStatusLabel.Text = $"Round {e.RoundCounter + 1} of {GuiRunnerSettings.Default.MaximumRounds + 1}";
                        });
                    }
                    break;
            }
        }
    }
}