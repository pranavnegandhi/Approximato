using Notadesigner.Tom.App.Properties;
using Notadesigner.Tom.Core;

namespace Notadesigner.Tom.App
{
    public class GuiRunnerContext : ApplicationContext
    {
        private readonly PomoEngine _engine;

        private readonly MainForm _form;

        private readonly NotifyIcon _notifyIcon;

        private readonly ContextMenuStrip _contextMenu = new();

        private readonly ToolStripMenuItem _startMenu;

        private readonly Dictionary<EngineState, IGuiState> EngineGuiStateMap = new();

        private IGuiState _guiState;

        public GuiRunnerContext(PomoEngine engine, MainForm form)
        {
            _engine = engine;
            _engine.Progress += (s, e) => _notifyIcon.Text = $"{e.ElapsedDuration:mm\\:ss} / {e.TotalDuration:mm\\:ss}"; ;
            _engine.StateChange += EngineStateChangeHandler;

            _form = form;
            _form.FormClosing += FormClosingHandler;

            _startMenu = new ToolStripMenuItem("&Start");
            _startMenu.Click += (s, e) => _engine.MoveNext();
            _contextMenu.Items.Add(_startMenu);

            _contextMenu.Items.Add(new ToolStripSeparator());

            var exitMenu = new ToolStripMenuItem("E&xit");
            exitMenu.Click += (s, e) => ExitThread();
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
                _form.Show();
            }
        }

        private void FormClosingHandler(object? sender, FormClosingEventArgs e)
        {
            _form.Hide();
            e.Cancel = true;
        }

        private void EngineStateChangeHandler(object? sender, StateChangeEventArgs e)
        {
            if (!EngineGuiStateMap.TryGetValue(e.State, out var guiState))
            {
                return;
            }

            _guiState = guiState;
            _guiState.Enter(e.RoundCounter);
        }
    }
}