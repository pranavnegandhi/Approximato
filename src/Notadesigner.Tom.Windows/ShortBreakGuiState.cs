namespace Notadesigner.Tom.App
{
    public class ShortBreakGuiState : IGuiState
    {
        private readonly ToolStripMenuItem _startMenu;

        private readonly ToolStripMenuItem _continueMenu;

        private readonly ToolStripMenuItem _resetMenu;

        public ShortBreakGuiState(ToolStripMenuItem startMenu, ToolStripMenuItem continueMenu, ToolStripMenuItem resetMenu)
        {
            _startMenu = startMenu;
            _continueMenu = continueMenu;
            _resetMenu = resetMenu;
        }

        public GuiState State => GuiState.WorkSession;

        public void Enter(int roundCounter)
        {
            _startMenu.Owner.Invoke(() =>
            {
                _startMenu.Enabled = false;
                _continueMenu.Enabled = false;
                _resetMenu.Enabled = true;
            });
        }
    }
}