namespace Notadesigner.Tom.App
{
    public interface IGuiState
    {
        void Enter(int roundCounter);

        GuiState State { get; }
    }
}