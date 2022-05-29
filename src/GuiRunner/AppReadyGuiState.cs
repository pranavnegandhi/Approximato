namespace Notadesigner.Tom.App
{
    public class AppReadyGuiState : IGuiState
    {
        private readonly ToolStripMenuItem _startMenu;

        public AppReadyGuiState(ToolStripMenuItem startMenu)
        {
            _startMenu = startMenu;
        }

        public GuiState State => GuiState.AppReady;

        public void Enter(int roundCounter)
        {
            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = true;
            });
        }
    }
}