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

        private int _roundCounter = 0;

        private TimerState _timerState = TimerState.Begin;

        public MainForm()
        {
            InitializeComponent();

            Icon = GuiRunnerResources.MainIcon;

            progressBarsContainer.Controls.Add(_workProgressBar);
            progressBarsContainer.Controls.Add(_breakProgressBar);

            _activeProgressBar = _workProgressBar;
            SetTimerState(TimerState.Begin);

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
                if (_elapsedDuration == value)
                {
                    return;
                }

                _elapsedDuration = value;
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
                if (_totalDuration == value)
                {
                    return;
                }

                _totalDuration = value;
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
            get => _roundCounter;

            set
            {
                if (_roundCounter == value)
                {
                    return;
                }

                _roundCounter = value;
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

        public TimerState TimerState
        {
            get => _timerState;

            set
            {
                if (_timerState == value)
                {
                    return;
                }

                _timerState = value;
                if (InvokeRequired)
                {
                    Invoke(() => SetTimerState(value));
                }
                else
                {
                    SetTimerState(value);
                }
            }
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

        private void SetTimerState(TimerState value)
        {
            switch (value)
            {
                case TimerState.Begin:
                case TimerState.Abandoned:
                    _activeProgressBar = null;
                    _workProgressBar.Text = "__:__ / __:__";
                    _workProgressBar.Value = 0;

                    _breakProgressBar.Text = "__:__ / __:__";
                    _breakProgressBar.Value = 0;
                    break;

                case TimerState.Focused:
                    _activeProgressBar = _workProgressBar;
                    ElapsedDuration = TimeSpan.Zero;
                    TotalDuration = TimeSpan.Zero;
                    break;

                case TimerState.Interrupted:
                    break;

                case TimerState.Finished:
                case TimerState.Refreshed:
                    _activeProgressBar = null;
                    break;

                case TimerState.Relaxed:
                case TimerState.Stopped:
                    _activeProgressBar = _breakProgressBar;
                    ElapsedDuration = TimeSpan.Zero;
                    TotalDuration = TimeSpan.Zero;
                    break;

                case TimerState.End:
                    break;
            }
        }

        private void SetRoundCounter(int value)
        {
            currentPhaseStatusLabel.Text = $"Round {value + 1} of {GuiRunnerSettings.Default.MaximumRounds + 1}";
        }
    }
}