namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    public static Guid Id => Guid.NewGuid();
    public static DateTime OcurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName!;
}
