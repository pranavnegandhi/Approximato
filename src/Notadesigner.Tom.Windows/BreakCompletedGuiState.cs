using Notadesigner.Tom.App.Properties;

namespace Notadesigner.Tom.App
{
    public class BreakCompletedGuiState : IGuiState
    {
        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _continueMenu;

        private readonly ToolStripMenuItem _resetMenu;

        private readonly NotifyIcon _notifyIcon;

        public BreakCompletedGuiState(ToolStripMenuItem startMenu, ToolStripMenuItem continueMenu, ToolStripMenuItem resetMenu, NotifyIcon notifyIcon)
        {
            _startMenu = startMenu;
            _continueMenu = continueMenu;
            _resetMenu = resetMenu;
            _notifyIcon = notifyIcon;
        }

        public GuiState State => GuiState.BreakCompleted;

        public void Enter(int roundCounter)
        {
            var message = GuiRunnerResources.BREAK_COMPLETED;

            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = false;
                _continueMenu.Enabled = true;
                _resetMenu.Enabled = true;
                _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
            });
        }
    }
}