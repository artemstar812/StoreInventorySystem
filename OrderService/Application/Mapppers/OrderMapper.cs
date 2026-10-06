using OrderService.Application.DTOs.Order;
using OrderService.Domain.Entity;

namespace OrderService.Application.Mapppers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                ProductId = order.ProductId,
                UserId = order.UserId,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
