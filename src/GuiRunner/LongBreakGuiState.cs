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

        public void Enter(int roundCounter)
        {
            _notifyIcon.ShowBalloonTip(500, string.Empty, "Your last work session is complete. Take a longer break to rejuvenate.", ToolTipIcon.None);
        }
    }
}