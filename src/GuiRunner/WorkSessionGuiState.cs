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
            _notifyIcon.ContextMenuStrip.Items[0].Enabled = false;
            _notifyIcon.ShowBalloonTip(500, string.Empty, $"Your {roundCounter} work session has begun.", ToolTipIcon.Info);
        }
    }
}