using Notadesigner.Tom.App.Properties;

namespace Notadesigner.Tom.App
{
    public class LongBreakGuiState : IGuiState
    {
        private readonly NotifyIcon _notifyIcon;

        public LongBreakGuiState(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int _)
        {
            var message = GuiRunnerResources.LONG_BREAK_3;
            _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
        }
    }
}