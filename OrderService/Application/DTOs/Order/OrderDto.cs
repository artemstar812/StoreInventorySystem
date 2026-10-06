using OrderService.Domain.Enums;

namespace OrderService.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
