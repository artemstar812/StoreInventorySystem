namespace Shared.Contracts.Events.Interfaces
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        string Topic { get; }
    }
}
