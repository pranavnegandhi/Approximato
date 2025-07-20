using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Approximato.Windows.Properties;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;

namespace Notadesigner.Approximato.Windows
{
    public class GuiRunnerContext : ApplicationContext, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IProducer<UIEvent> _uiEventProducer;

        private readonly SettingsForm _settingsForm;

        private readonly NotifyIcon _notifyIcon;

        private readonly ContextMenuStrip _contextMenu = new();

        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _interruptMenu;

        private readonly ToolStripMenuItem _resumeMenu;

        private readonly ToolStripMenuItem _continueMenu;

        private readonly ToolStripMenuItem _abandonMenu;

        private readonly ToolStripMenuItem _resetMenu;

        private TimeSpan _elapsedDuration;

        private TimeSpan _totalDuration;

        private int _focusCounter;

        private TimerState _timerState;

        private readonly SoundPlayer _tickPlayer;

        private readonly SoundPlayer _enterSound;

        private readonly SoundPlayer _exitSound;

        public GuiRunnerContext(IProducer<UIEvent> uiEventProducer,
            [FromKeyedServices("guiTransition")] IEventHandler<TransitionEvent> transitionHandler,
            [FromKeyedServices("guiTimer")] IEventHandler<TimerEvent> timerHandler,
            MainForm mainForm,
            SettingsForm settingsForm)
        {
            _notifyIcon = new NotifyIcon
            {
                ContextMenuStrip = _contextMenu,
                Icon = GuiRunnerResources.MainIcon,
                Visible = true
            };

            _uiEventProducer = uiEventProducer;
            var handler = (GuiTransitionEventHandler)transitionHandler;
            handler.Abandoned += AbandonedEventHandler;
            handler.Begin += BeginEventHandler;
            handler.End += EndEventHandler;
            handler.Finished += FinishedEventHandler;
            handler.FocusedEntry += FocusedEventHandler;
            handler.FocusedExit += FocusedExitEventHandler;
            handler.Interrupted += InterruptedEventHandler;
            handler.Refreshed += RefreshedEventHandler;
            handler.Relaxed += RelaxedEventHandler;
            handler.Stopped += StoppedEventHandler;

            timerHandler.EventReceived += TimerEventReceivedHandler;

            MainForm = mainForm;
            MainForm.FormClosing += FormClosingHandler;

            _settingsForm = settingsForm;

            _startMenu = new ToolStripMenuItem("&Start");
            _startMenu.Click += async (s, e) => await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Focus)));
            _contextMenu.Items.Add(_startMenu);

            _interruptMenu = new ToolStripMenuItem("&Interrupt");
            _interruptMenu.Click += async (s, e) => await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Interrupt)));
            _contextMenu.Items.Add(_interruptMenu);

            _resumeMenu = new ToolStripMenuItem("&Resume");
            _resumeMenu.Click += async (s, e) => await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Resume)));
            _contextMenu.Items.Add(_resumeMenu);

            _continueMenu = new ToolStripMenuItem("&Continue");
            _continueMenu.Click += async (s, e) => await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Continue)));
            _contextMenu.Items.Add(_continueMenu);

            _abandonMenu = new ToolStripMenuItem("&Abandon…");
            _abandonMenu.Enabled = false;
            _abandonMenu.Click += AbandonMenuClickHandlerAsync;
            _contextMenu.Items.Add(_abandonMenu);

            _resetMenu = new ToolStripMenuItem("&Reset");
            _resetMenu.Enabled = false;
            _resetMenu.Click += async (s, e) => await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Reset)));
            _contextMenu.Items.Add(_resetMenu);

            var settingsMenu = new ToolStripMenuItem("S&ettings…");
            settingsMenu.Click += SettingsClickHandler;
            _contextMenu.Items.Add(settingsMenu);

            _contextMenu.Items.Add(new ToolStripSeparator());

            var exitMenu = new ToolStripMenuItem("E&xit…");
            exitMenu.Click += ExitMenuClickHandler;
            _contextMenu.Items.Add(exitMenu);

            var binding = new Binding(nameof(Windows.MainForm.ElapsedDuration), this, nameof(ElapsedDuration), false);
            MainForm.DataBindings.Add(binding);

            binding = new Binding(nameof(Windows.MainForm.TotalDuration), this, nameof(TotalDuration), false);
            MainForm.DataBindings.Add(binding);

            binding = new Binding(nameof(Windows.MainForm.FocusCounter), this, nameof(FocusCounter), false);
            MainForm.DataBindings.Add(binding);

            binding = new Binding(nameof(Windows.MainForm.TimerState), this, nameof(TimerState), false);
            MainForm.DataBindings.Add(binding);
            TimerState = TimerState.Begin;

            _tickPlayer = new(GuiRunnerResources.Tick);
            _enterSound = new(GuiRunnerResources.Ding);
            _exitSound = new(GuiRunnerResources.DingDing);

            _notifyIcon.MouseClick += NotifyIconMouseClickHandler;
            BeginEventHandler(null, 0);
        }

        private void AbandonedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Abandoned;
            FocusCounter = e;
            /// What to do when the pomodoro is abandoned?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = false;
            _resetMenu.Enabled = true;
            _notifyIcon.Text = GuiRunnerResources.AbandonedToolTip;
        }

        private void BeginEventHandler(object? _, int e)
        {
            TimerState = TimerState.Begin;
            FocusCounter = e;
            /// What to do before the pomodoro begins?
            _startMenu.Enabled = true;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = false;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = GuiRunnerResources.BeginToolTip;
        }

        private void EndEventHandler(object? _, int e)
        {
            TimerState = TimerState.End;
            FocusCounter = e;
            /// What to do when the pomodoro ends?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = false;
            _resetMenu.Enabled = true;
            _notifyIcon.Text = GuiRunnerResources.EndToolTip;
        }

        private void FinishedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Finished;
            FocusCounter = e;
            /// What to do when a focus is finished?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
            _abandonMenu.Enabled = true;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;
        }

        private void FocusedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Focused;
            FocusCounter = e;
            /// What to do when entering a focus?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = true;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = true;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;

            _enterSound.PlaySync();
            _tickPlayer.PlayLooping();
        }

        private void FocusedExitEventHandler(object? _, EventArgs _1)
        {
            _tickPlayer.Stop();
            _exitSound.PlaySync();
        }

        private void InterruptedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Interrupted;
            FocusCounter = e;
            /// What to do when the pomodoro is interrupted?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = true;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = false;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;
        }

        private void RefreshedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Refreshed;
            FocusCounter = e;
            /// What to do when refreshed?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
            _abandonMenu.Enabled = true;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;

            var message = GuiRunnerResources.RefreshedEnterNotification;
            _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
        }

        private void RelaxedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Relaxed;
            FocusCounter = e;
            /// What to do when relaxed?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = false;
            _abandonMenu.Enabled = true;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;

            var message = GuiRunnerResources.RelaxedEnterNotification;
            _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
        }

        private void StoppedEventHandler(object? _, int e)
        {
            TimerState = TimerState.Relaxed;
            FocusCounter = e;
            /// What to do when all pomodoros are completed?
            _startMenu.Enabled = false;
            _interruptMenu.Enabled = false;
            _resumeMenu.Enabled = false;
            _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
            _abandonMenu.Enabled = true;
            _resetMenu.Enabled = false;
            _notifyIcon.Text = null;

            var message = GuiRunnerResources.StoppedEnterNotification;
            _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
        }

        public TimeSpan TotalDuration
        {
            get => _totalDuration;

            set
            {
                if (value != _totalDuration)
                {
                    _totalDuration = value;

                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan ElapsedDuration
        {
            get => _elapsedDuration;

            set
            {
                if (value != _elapsedDuration)
                {
                    _elapsedDuration = value;

                    string tooltip;
                    if (_elapsedDuration.TotalMinutes < 1)
                    {
                        tooltip = GuiRunnerResources.LessThanOneMinuteElapsedToolTip;
                    }
                    else if (_elapsedDuration.TotalMinutes > 1 && _elapsedDuration.TotalMinutes < 2)
                    {
                        tooltip = GuiRunnerResources.OneMinuteElapsedToolTip;
                    }
                    else
                    {
                        tooltip = string.Format(GuiRunnerResources.MoreThanOneMinuteElapsedToolTipTemplate, _elapsedDuration.TotalMinutes);
                    }
                    _notifyIcon.Text = tooltip;

                    OnPropertyChanged();
                }
            }
        }

        public int FocusCounter
        {
            get => _focusCounter;

            set
            {
                if (value != _focusCounter)
                {
                    _focusCounter = value;

                    OnPropertyChanged();
                }
            }
        }

        public TimerState TimerState
        {
            get => _timerState;

            set
            {
                if (value != _timerState)
                {
                    _timerState = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _notifyIcon.Dispose();
        }

        private void NotifyIconMouseClickHandler(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MainForm?.Show();
                MainForm?.BringToFront();
            }
        }

        protected override void OnMainFormClosed(object? sender, EventArgs e)
        {
        }

        private void FormClosingHandler(object? sender, FormClosingEventArgs e)
        {
            MainForm?.Hide();
            e.Cancel = true;
        }

        private async void AbandonMenuClickHandlerAsync(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Abandon the Pomodoro?", "Tom", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await _uiEventProducer.PublishAsync(new Event<UIEvent>(new UIEvent(TimerTrigger.Abandon)));
            }
        }

        private void SettingsClickHandler(object? sender, EventArgs e)
        {
            if (_settingsForm.Visible)
            {
                _settingsForm.Focus();
            }
            else
            {
                _settingsForm.ShowDialog();
            }
        }

        private void ExitMenuClickHandler(object? sender, EventArgs e)
        {
            var result = DialogResult.Yes;

            if (TimerState != TimerState.Begin)
            {
                result = MessageBox.Show("Halt Pomodoro and exit?", "Tom", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (result == DialogResult.Yes)
            {
                ExitThread();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TimerEventReceivedHandler(object? sender, TimerEvent @event)
        {
            ElapsedDuration = @event.Elapsed;
            TotalDuration = @event.TotalDuration;
        }
    }
}