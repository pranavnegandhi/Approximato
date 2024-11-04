using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Approximato.Windows.Properties;
using Stateless;
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

        private readonly StateMachine<TimerState, TimerTrigger> _stateMachine;

        public GuiRunnerContext(IProducer<UIEvent> uiEventProducer,
            [FromKeyedServices("guiTransition")] IEventHandler<TransitionEvent> transitionHandler,
            [FromKeyedServices("guiTimer")] IEventHandler<TimerEvent> timerHandler,
            MainForm mainForm,
            SettingsForm settingsForm)
        {
            _stateMachine = new StateMachine<TimerState, TimerTrigger>(TimerState.Begin);
            ConfigureStates(_stateMachine);

            _notifyIcon = new NotifyIcon
            {
                ContextMenuStrip = _contextMenu,
                Icon = GuiRunnerResources.MainIcon,
                Visible = true
            };

            _uiEventProducer = uiEventProducer;
            transitionHandler.EventReceived += TransitionEventReceivedHandler;
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

            _stateMachine.Fire(TimerTrigger.Reset);
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
                    var oldTimerState = _timerState;
                    _timerState = value;

                    switch (_timerState)
                    {
                        case TimerState.Abandoned:
                            _stateMachine.Fire(TimerTrigger.Abandon);
                            break;

                        case TimerState.Begin:
                            _stateMachine.Fire(TimerTrigger.Reset);
                            break;

                        case TimerState.End:
                            _stateMachine.Fire(TimerTrigger.Timeout);
                            break;

                        case TimerState.Finished:
                            _stateMachine.Fire(TimerTrigger.Timeout);
                            break;

                        case TimerState.Focused:
                            if (oldTimerState == TimerState.Begin)
                            {
                                _stateMachine.Fire(TimerTrigger.Focus);
                            }
                            else if (oldTimerState == TimerState.Interrupted)
                            {
                                _stateMachine.Fire(TimerTrigger.Resume);
                            }
                            else if (oldTimerState == TimerState.Refreshed)
                            {
                                _stateMachine.Fire(TimerTrigger.Continue);
                            }
                            break;

                        case TimerState.Interrupted:
                            _stateMachine.Fire(TimerTrigger.Interrupt);
                            break;

                        case TimerState.Refreshed:
                            _stateMachine.Fire(TimerTrigger.Timeout);
                            break;

                        case TimerState.Relaxed:
                        case TimerState.Stopped:
                            _stateMachine.Fire(TimerTrigger.Continue);
                            break;
                    }

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

        private void TransitionEventReceivedHandler(object? sender, TransitionEvent @event)
        {
            FocusCounter = @event.FocusCounter;
            TimerState = @event.TimerState;
        }

        private void TimerEventReceivedHandler(object? sender, TimerEvent @event)
        {
            ElapsedDuration = @event.Elapsed;
            TotalDuration = @event.TotalDuration;
        }

        private void ConfigureStates(StateMachine<TimerState, TimerTrigger> stateMachine)
        {
            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            stateMachine.Configure(TimerState.Abandoned)
                .OnEntry(() =>
                {
                    /// What to do when the pomodoro is abandoned?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = false;
                    _resetMenu.Enabled = true;
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Begin)
                .OnEntry(() =>
                {
                    /// What to do before the pomodoro begins?
                    _startMenu.Enabled = true;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = false;
                    _resetMenu.Enabled = false;
                })
                .Permit(TimerTrigger.Focus, TimerState.Focused)
                .PermitReentry(TimerTrigger.Reset); /// Explicitly allowed to easily set UI state on application startup

            stateMachine.Configure(TimerState.End)
                .OnEntry(() =>
                {
                    /// What to do when the pomodoro ends?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = false;
                    _resetMenu.Enabled = true;
                })
                .Permit(TimerTrigger.Reset, TimerState.Begin);

            stateMachine.Configure(TimerState.Finished)
                .OnEntry(() =>
                {
                    /// What to do when a focus is finished?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
                    _abandonMenu.Enabled = true;
                    _resetMenu.Enabled = false;
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .PermitIf(TimerTrigger.Continue, TimerState.Stopped, () => TimerState == TimerState.Stopped)
                .PermitIf(TimerTrigger.Continue, TimerState.Relaxed, () => TimerState == TimerState.Relaxed);

            stateMachine.Configure(TimerState.Focused)
                .OnEntry(() =>
                {
                    /// What to do when entering a focus?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = true;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = true;
                    _resetMenu.Enabled = false;

                    var message = GuiRunnerResources.FocusedEnterNotification;
                    _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);

                    _ = Task.Run(() => _enterSound.PlaySync())
                        .ContinueWith(state => _tickPlayer.PlayLooping());
                })
                .OnExit(() =>
                {
                    _tickPlayer.Stop();
                    _exitSound.PlaySync();
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Interrupt, TimerState.Interrupted)
                .Permit(TimerTrigger.Timeout, TimerState.Finished);

            stateMachine.Configure(TimerState.Interrupted)
                .OnEntry(() =>
                {
                    /// What to do when the pomodoro is interrupted?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = true;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = false;
                    _resetMenu.Enabled = false;
                })
                .Permit(TimerTrigger.Resume, TimerState.Focused);

            stateMachine.Configure(TimerState.Refreshed)
                .OnEntry(() =>
                {
                    /// What to do when refreshed?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
                    _abandonMenu.Enabled = true;
                    _resetMenu.Enabled = false;

                    var message = GuiRunnerResources.RefreshedEnterNotification;
                    _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Continue, TimerState.Focused);

            stateMachine.Configure(TimerState.Relaxed)
                .OnEntry(() =>
                {
                    /// What to do when relaxed?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = false;
                    _abandonMenu.Enabled = true;
                    _resetMenu.Enabled = false;
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.Refreshed);

            stateMachine.Configure(TimerState.Stopped)
                .OnEntry(() =>
                {
                    /// What to do when all pomodoros are completed?
                    _startMenu.Enabled = false;
                    _interruptMenu.Enabled = false;
                    _resumeMenu.Enabled = false;
                    _continueMenu.Enabled = GuiRunnerSettings.Default.LenientMode;
                    _abandonMenu.Enabled = true;
                    _resetMenu.Enabled = false;

                    var message = GuiRunnerResources.StoppedEnterNotification;
                    _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
                })
                .Permit(TimerTrigger.Abandon, TimerState.Abandoned)
                .Permit(TimerTrigger.Timeout, TimerState.End);
        }
    }
}