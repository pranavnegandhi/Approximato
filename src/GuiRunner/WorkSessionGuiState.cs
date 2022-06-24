using Notadesigner.Tom.App.Properties;
using System.Media;

namespace Notadesigner.Tom.App
{
    public class WorkSessionGuiState : IGuiState
    {
        private readonly NotifyIcon _notifyIcon;

        private readonly SoundPlayer _tickPlayer;

        private readonly SoundPlayer _enterSound;

        private readonly SoundPlayer _exitSound;

        public WorkSessionGuiState(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
            _tickPlayer = new(GuiRunnerResources.Tick);
            _enterSound = new(GuiRunnerResources.Ding);
            _exitSound = new(GuiRunnerResources.DingDing);
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int roundCounter)
        {
            var message = GuiRunnerResources.WORK_SESSION;

            _notifyIcon.ContextMenuStrip.Invoke(() =>
            {
                _notifyIcon.ShowBalloonTip(500, string.Empty, message, ToolTipIcon.None);
                _notifyIcon.ContextMenuStrip.Items[0].Enabled = false;
            });

            Task.Run(() => _enterSound.PlaySync())
                .ContinueWith(state => _tickPlayer.PlayLooping());
        }

        public void Exit() => _exitSound.Play();
    }
}