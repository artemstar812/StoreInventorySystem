using Shared.Contracts.Events.Interfaces;

namespace OrderService.Application.Interfaces
{
    public interface IEventProducer
    {
        Task PublishAsync<T> (T @event) where T : IIntegrationEvent;
    }
}
