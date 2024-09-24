using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using WindowsFormsLifetime;

namespace Notadesigner.Approximato.Windows;

public class GuiTransitionEventHandler(IGuiContext context) : IEventHandler<TransitionEvent>
{
    public event EventHandler<TransitionEvent>? EventReceived;

    ValueTask IEventHandler<TransitionEvent>.HandleAsync(TransitionEvent @event, CancellationToken cancellationToken)
    {
        context.Invoke(() => EventReceived?.Invoke(this, @event));

        return ValueTask.CompletedTask;
    }
}