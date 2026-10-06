using OrderService.Domain.Enums;

namespace OrderService.Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
    }
}
