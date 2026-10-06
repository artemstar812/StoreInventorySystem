using OrderService.Application.DTOs;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<OrderDto>> GetPagedAsync(int page, int pageSize);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateOrder(CreateOrderDto dto, int userId);
    }
}
