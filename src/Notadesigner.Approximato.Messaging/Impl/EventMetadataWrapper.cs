using Notadesigner.Approximato.Messaging.Contracts;

namespace Notadesigner.Approximato.Messaging.Impl
{
    internal sealed class EventMetadataWrapper<T>
    {
        public Event<T>? Event { get; set; }
    }
}