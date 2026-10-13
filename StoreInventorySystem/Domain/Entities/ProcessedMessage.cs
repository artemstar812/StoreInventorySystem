namespace InventorySystem.Domain.Entities
{
    public class ProcessedMessage
    {
        public Guid EventId { get; set; }
        public int? OrderId { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
