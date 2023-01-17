namespace Notadesigner.Tom.App
{
    public class AppReadyGuiState : IGuiState
    {
        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _continueMenu;

        private readonly ToolStripMenuItem _resetMenu;

        public AppReadyGuiState(ToolStripMenuItem startMenu, ToolStripMenuItem continueMenu, ToolStripMenuItem resetMenu)
        {
            _startMenu = startMenu;
            _continueMenu = continueMenu;
            _resetMenu = resetMenu;
        }

        public GuiState State => GuiState.AppReady;

        public void Enter(int roundCounter)
        {
            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = true;
                _continueMenu.Enabled = false;
                _resetMenu.Enabled = false;
            });
        }
    }
}