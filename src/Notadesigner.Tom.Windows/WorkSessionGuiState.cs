using Notadesigner.Tom.App.Properties;
using System.Media;

namespace Notadesigner.Tom.App
{
    public class WorkSessionGuiState : IGuiState
    {
        private readonly NotifyIcon _notifyIcon;

        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _continueMenu;

        private readonly ToolStripMenuItem _resetMenu;

        private readonly SoundPlayer _tickPlayer;

        private readonly SoundPlayer _enterSound;

        private readonly SoundPlayer _exitSound;

        public WorkSessionGuiState(ToolStripMenuItem startMenu, ToolStripMenuItem continueMenu, ToolStripMenuItem resetMenu, NotifyIcon notifyIcon)
        {
            _startMenu = startMenu;
            _continueMenu = continueMenu;
            _resetMenu = resetMenu;
            _notifyIcon = notifyIcon;
            _tickPlayer = new(GuiRunnerResources.Tick);
            _enterSound = new(GuiRunnerResources.Ding);
            _exitSound = new(GuiRunnerResources.DingDing);
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int roundCounter)
        {
            var message = GuiRunnerResources.WORK_SESSION;

            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = false;
                _continueMenu.Enabled = false;
                _resetMenu.Enabled = true;
                _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
            });

            Task.Run(() => _enterSound.PlaySync())
                .ContinueWith(state => _tickPlayer.PlayLooping());
        }

        public void Exit() => _exitSound.Play();
    }
}