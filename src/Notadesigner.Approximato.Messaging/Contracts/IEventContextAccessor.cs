namespace Notadesigner.Approximato.Messaging.Contracts
{
    public interface IEventContextAccessor<T>
    {
        Event<T>? Event { get; }

        void Set(Event<T> @event);
    }
}