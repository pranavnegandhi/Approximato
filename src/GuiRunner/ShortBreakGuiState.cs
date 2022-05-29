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
            string message;

            if (roundCounter == 0)
            {
                message = GuiRunnerResources.SHORT_BREAK_0;
            }
            else if (roundCounter == 1)
            {
                message = GuiRunnerResources.SHORT_BREAK_1;
            }
            else
            {
                message = GuiRunnerResources.SHORT_BREAK_2;
            }

            _notifyIcon.ContextMenuStrip.Invoke(() =>
            {
                _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
            });
        }
    }
}