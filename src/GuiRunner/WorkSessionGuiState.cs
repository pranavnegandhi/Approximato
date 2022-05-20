using Notadesigner.Tom.App.Properties;

namespace Notadesigner.Tom.App
{
    public class WorkSessionGuiState : IGuiState
    {
        private readonly NotifyIcon _notifyIcon;

        public WorkSessionGuiState(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int roundCounter)
        {
            string message;

            if (roundCounter == 0)
            {
                message = GuiRunnerResources.WORK_SESSION_0;
            }
            else if (roundCounter == 1)
            {
                message = GuiRunnerResources.WORK_SESSION_1;
            }
            else if (roundCounter == 2)
            {
                message = GuiRunnerResources.WORK_SESSION_2;
            }
            else
            {
                message = GuiRunnerResources.WORK_SESSION_3;
            }

            _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
            _notifyIcon.ContextMenuStrip.Items[0].Enabled = false;
        }
    }
}