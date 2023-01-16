namespace Notadesigner.Tom.App
{
    public class AppReadyGuiState : IGuiState
    {
        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _resetMenu;

        public AppReadyGuiState(ToolStripMenuItem startMenu, ToolStripMenuItem resetMenu)
        {
            _startMenu = startMenu;
            _resetMenu = resetMenu;
        }

        public GuiState State => GuiState.AppReady;

        public void Enter(int roundCounter)
        {
            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = true;
                _resetMenu.Enabled = false;
            });
        }
    }
}