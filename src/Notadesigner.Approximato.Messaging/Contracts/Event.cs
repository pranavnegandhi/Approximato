namespace Notadesigner.Approximato.Messaging.Contracts
{
    public record EventMetadata(string CorrelationId);

    public record Event<T>(T Data, EventMetadata? Metadata = default);
}