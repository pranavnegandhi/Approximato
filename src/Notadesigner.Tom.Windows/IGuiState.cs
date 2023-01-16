namespace Notadesigner.Tom.App
{
    public interface IGuiState
    {
        void Enter(int roundCounter);

        public void Exit()
        {
        }

        GuiState State { get; }
    }
}