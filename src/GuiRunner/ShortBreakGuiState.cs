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
            _notifyIcon.ShowBalloonTip(500, string.Empty, $"Time for your {roundCounter} short break.", ToolTipIcon.None);
        }
    }
}