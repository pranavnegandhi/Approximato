using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using WindowsFormsLifetime;

namespace Notadesigner.Approximato.Windows;

public class GuiTimerEventHandler(IGuiContext context) : IEventHandler<TimerEvent>
{
    public event EventHandler<TimerEvent>? EventReceived;

    ValueTask IEventHandler<TimerEvent>.HandleAsync(TimerEvent @event, CancellationToken cancellationToken)
    {
        context.Invoke(() => EventReceived?.Invoke(this, @event));

        return ValueTask.CompletedTask;
    }
}