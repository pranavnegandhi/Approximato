using Notadesigner.Tom.App.Properties;

namespace Notadesigner.Tom.App
{
    public class ShortBreakGuiState : IGuiState
    {
        private readonly NotifyIcon _notifyIcon;

        public ShortBreakGuiState(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int roundCounter)
        {
            var message = GuiRunnerResources.SHORT_BREAK;

            _notifyIcon.ContextMenuStrip.Invoke(() =>
            {
                _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
            });
        }
    }
}