using OrderService.Domain.Entity;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<(List<Order>, int)> GetPagedAsync(int page, int pageSize);
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
    }
}
