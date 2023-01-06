using Notadesigner.Tom.App.Properties;
using Notadesigner.Tom.Core;

namespace Notadesigner.Tom.App
{
    public partial class MainForm : Form
    {
        private readonly CircularProgressBar.CircularProgressBar _workProgressBar = ProgressBarFactory.Create(SystemColors.Highlight, new Size(120, 120));

        private readonly CircularProgressBar.CircularProgressBar _breakProgressBar = ProgressBarFactory.Create(SystemColors.GradientActiveCaption, new Size(100, 100));

        private CircularProgressBar.CircularProgressBar? _activeProgressBar;

        private TimeSpan _elapsedDuration = TimeSpan.Zero;

        private TimeSpan _totalDuration = TimeSpan.Zero;

        public MainForm()
        {
            InitializeComponent();

            Icon = GuiRunnerResources.MainIcon;

            progressBarsContainer.Controls.Add(_workProgressBar);
            progressBarsContainer.Controls.Add(_breakProgressBar);

            _activeProgressBar = _workProgressBar;
            SetEngineState(EngineState.AppReady);

            VisibleChanged += (s, e) =>
            {
                if (Visible)
                {
                    var rect = Screen.PrimaryScreen.WorkingArea;
                    Location = new Point(rect.Right - Width, rect.Bottom - Height);
                }
            };
        }

        public TimeSpan ElapsedDuration
        {
            get => _elapsedDuration;

            set
            {
                if (InvokeRequired)
                {
                    Invoke(() => SetElapsedDuration(value));
                }
                else
                {
                    SetElapsedDuration(value);
                }
            }
        }

        public TimeSpan TotalDuration
        {
            get => _totalDuration;

            set
            {
                if (InvokeRequired)
                {
                    Invoke(() => SetTotalDuration(value));
                }
                else
                {
                    SetTotalDuration(value);
                }
            }
        }

        public int RoundCounter
        {
            set
            {
                if (InvokeRequired)
                {
                    Invoke(() => SetRoundCounter(value));
                }
                else
                {
                    SetRoundCounter(value);
                }
            }
        }

        public EngineState EngineState
        {
            set
            {
                if (InvokeRequired)
                {
                    Invoke(() => SetEngineState(value));
                }
                else
                {
                    SetEngineState(value);
                }
            }
        }

        private void EngineProgressHandler(object? sender, ProgressEventArgs e)
        {
            if (_activeProgressBar is null || !_activeProgressBar.IsHandleCreated)
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

        private void SetElapsedDuration(TimeSpan value)
        {
            if (_activeProgressBar is null ||
                !_activeProgressBar.IsHandleCreated ||
                value > _totalDuration)
            {
                return;
            }

            _elapsedDuration = value;
            _activeProgressBar.Value = Convert.ToInt32(value.TotalSeconds);
            _activeProgressBar.Text = $"{_elapsedDuration:mm\\:ss} / {_totalDuration:mm\\:ss}";
        }

        private void SetTotalDuration(TimeSpan value)
        {
            if (_activeProgressBar is null ||
                !_activeProgressBar.IsHandleCreated ||
                value < _elapsedDuration)
            {
                return;
            }

            _totalDuration = value;
            _activeProgressBar.Maximum = Convert.ToInt32(value.TotalSeconds);
            _activeProgressBar.Text = $"{_elapsedDuration:mm\\:ss} / {_totalDuration:mm\\:ss}";
        }

        private void SetEngineState(EngineState value)
        {
            switch (value)
            {
                case EngineState.AppReady:
                    _activeProgressBar = null;
                    _workProgressBar.Text = "__:__ / __:__";
                    _workProgressBar.Value = 0;

                    _breakProgressBar.Text = "__:__ / __:__";
                    _breakProgressBar.Value = 0;
                    break;

                case EngineState.LongBreak:
                case EngineState.ShortBreak:
                    _activeProgressBar = _breakProgressBar;
                    _breakProgressBar.Text = "00:00 / 00:00";
                    break;

                case EngineState.WorkSession:
                    _activeProgressBar = _workProgressBar;
                    _workProgressBar.Text = "__:__ / __:__";
                    _workProgressBar.Value = 0;

                    _breakProgressBar.Text = "__:__ / __:__";
                    _breakProgressBar.Value = 0;
                    break;
            }
        }

        private void SetRoundCounter(int value)
        {
            currentPhaseStatusLabel.Text = $"Round {value + 1} of {GuiRunnerSettings.Default.MaximumRounds + 1}";
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