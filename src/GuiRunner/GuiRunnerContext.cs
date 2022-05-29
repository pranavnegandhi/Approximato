using Notadesigner.Tom.App.Properties;
using Notadesigner.Tom.Core;
using System.Diagnostics;

namespace Notadesigner.Tom.App
{
    public class GuiRunnerContext : ApplicationContext
    {
        private readonly PomoEngine _engine;

        private readonly MainForm _mainForm;

        private readonly SettingsForm _settingsForm;

        private readonly NotifyIcon _notifyIcon;

        private readonly ContextMenuStrip _contextMenu = new();

        private readonly ToolStripMenuItem _startMenu;

        private readonly Dictionary<EngineState, IGuiState> EngineGuiStateMap = new();

        private IGuiState _guiState;

        public GuiRunnerContext(PomoEngine engine, MainForm mainForm, SettingsForm settingsForm)
        {
            _engine = engine;
            _engine.Progress += (s, e) => _notifyIcon.Text = $"{e.ElapsedDuration:mm\\:ss} / {e.TotalDuration:mm\\:ss}"; ;
            _engine.StateChange += EngineStateChangeHandler;

            _mainForm = mainForm;
            _mainForm.FormClosing += FormClosingHandler;

            _settingsForm = settingsForm;

            _startMenu = new ToolStripMenuItem("&Start");
            _startMenu.Click += (s, e) => _engine.MoveNext();
            _contextMenu.Items.Add(_startMenu);

            var settingsMenu = new ToolStripMenuItem("S&ettings…");
            settingsMenu.Click += SettingsClickHandler;
            _contextMenu.Items.Add(settingsMenu);

            _contextMenu.Items.Add(new ToolStripSeparator());

            var exitMenu = new ToolStripMenuItem("E&xit");
            exitMenu.Click += ExitMenuClickHandler;
            _contextMenu.Items.Add(exitMenu);

            _notifyIcon = new NotifyIcon
            {
                ContextMenuStrip = _contextMenu,
                Icon = GuiRunnerResources.MainIcon,
                Visible = true
            };

            _notifyIcon.MouseClick += NotifyIconMouseClickHandler;

            EngineGuiStateMap.Add(EngineState.AppReady, new AppReadyGuiState(_startMenu));
            EngineGuiStateMap.Add(EngineState.WorkSession, new WorkSessionGuiState(_notifyIcon));
            EngineGuiStateMap.Add(EngineState.ShortBreak, new ShortBreakGuiState(_notifyIcon));
            EngineGuiStateMap.Add(EngineState.LongBreak, new LongBreakGuiState(_notifyIcon));

            _guiState = EngineGuiStateMap[EngineState.AppReady];
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
                _mainForm.Show();
                _mainForm.BringToFront();
            }
        }

        private void FormClosingHandler(object? sender, FormClosingEventArgs e)
        {
            _mainForm.Hide();
            e.Cancel = true;
        }

        private void EngineStateChangeHandler(object? sender, StateChangeEventArgs e)
        {
            switch (e.State)
            {
                case EngineState.BreakCompleted:
                    if (GuiRunnerSettings.Default.AutoAdvance)
                    {
                        _engine.MoveNext();
                    }
                    else
                    {
                    }

                    break;

                case EngineState.WorkCompleted:
                    if (GuiRunnerSettings.Default.AutoAdvance)
                    {
                        _engine.MoveNext();
                    }
                    else
                    {
                    }

                    break;
            }

            if (!EngineGuiStateMap.TryGetValue(e.State, out var guiState))
            {
                return;
            }

            _guiState.Exit();
            _guiState = guiState;
            _mainForm.Invoke(() => _guiState.Enter(e.RoundCounter));
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

            if (_engine.State != EngineState.AppReady)
            {
                result = MessageBox.Show("Halt Pomodoro and exit?", "Tom", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (result == DialogResult.Yes)
            {
                ExitThread();
            }
        }
    }
}