using Shared.Contracts.Events.Interfaces;

namespace Shared.Contracts.Events
{
    public class OrderCreatedEvent : IIntegrationEvent
    {
        public Guid EventId { get; set; }
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice {  get; set; }

        public string Topic => "order-created";
    }
}
