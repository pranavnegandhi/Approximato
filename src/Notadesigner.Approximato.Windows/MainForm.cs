using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Windows.Properties;

namespace Notadesigner.Approximato.Windows
{
    public partial class MainForm : Form
    {
        private readonly CircularProgressBar.CircularProgressBar _workProgressBar = ProgressBarFactory.Create(SystemColors.Highlight, new Size(120, 120));

        private readonly CircularProgressBar.CircularProgressBar _breakProgressBar = ProgressBarFactory.Create(SystemColors.GradientActiveCaption, new Size(100, 100));

        private readonly IReadOnlyDictionary<TimerState, Image> TimerStateIcons = new Dictionary<TimerState, Image>()
        {
            { TimerState.Abandoned, GuiRunnerResources.TimerStateAbandoned },
            { TimerState.End, GuiRunnerResources.TimerStateStopped },
            { TimerState.Finished, GuiRunnerResources.TimerStateRelaxed },
            { TimerState.Focused, GuiRunnerResources.TimerStateFocused },
            { TimerState.Interrupted, GuiRunnerResources.TimerStateInterrupted },
            { TimerState.Refreshed, GuiRunnerResources.TimerStateRefreshed },
            { TimerState.Relaxed, GuiRunnerResources.TimerStateRelaxed },
            { TimerState.Stopped, GuiRunnerResources.TimerStateStopped }
        };

        private CircularProgressBar.CircularProgressBar? _activeProgressBar;

        private TimeSpan _elapsedDuration = TimeSpan.Zero;

        private TimeSpan _totalDuration = TimeSpan.Zero;

        private int _focusCounter = 0;

        private TimerState _timerState = TimerState.Begin;

        public MainForm()
        {
            InitializeComponent();

            Icon = GuiRunnerResources.MainIcon;

            progressBarsContainer.Controls.Add(_workProgressBar);
            progressBarsContainer.Controls.Add(_breakProgressBar);

            _activeProgressBar = _workProgressBar;

            VisibleChanged += (s, e) =>
            {
                if (Visible)
                {
                    var rect = Screen.PrimaryScreen?.WorkingArea ?? new(0, 0, 100, 100);
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

        public int FocusCounter
        {
            get => _focusCounter;

            set
            {
                if (_focusCounter == value)
                {
                    return;
                }

                _focusCounter = value;
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
                !_activeProgressBar.IsHandleCreated)
            {
                return;
            }

            _elapsedDuration = value;
            var elapsedSeconds = Math.Min(_elapsedDuration.TotalSeconds, _totalDuration.TotalSeconds);
            _activeProgressBar.Value = Convert.ToInt32(elapsedSeconds);
            _activeProgressBar.Text = $"{_elapsedDuration:mm\\:ss} / {_totalDuration:mm\\:ss}";
        }

        private void SetTotalDuration(TimeSpan value)
        {
            if (_activeProgressBar is null ||
                !_activeProgressBar.IsHandleCreated)
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

                    _breakProgressBar.Text = "__:__ / __:__";
                    _breakProgressBar.Value = 0;
                    break;

                case TimerState.Interrupted:
                    _activeProgressBar = _workProgressBar;
                    ElapsedDuration = TimeSpan.Zero;
                    TotalDuration = TimeSpan.FromMinutes(59);
                    break;

                case TimerState.Finished:
                    _activeProgressBar = _workProgressBar;
                    break;

                case TimerState.Refreshed:
                    _activeProgressBar = _breakProgressBar;
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

            if (TimerStateIcons.TryGetValue(value, out var stateIcon))
            {
                currentStateStatusLabel.Image = stateIcon;
            }
            else
            {
                currentStateStatusLabel.Image = null;
            }
        }

        private void SetRoundCounter(int value)
        {
            string message;

            if (value == 0)
            {
                message = string.Empty;
            }
            else
            {
                message = $"Round {value} of {GuiRunnerSettings.Default.MaximumRounds}";
            }

            currentPhaseStatusLabel.Text = message;
        }
    }
}