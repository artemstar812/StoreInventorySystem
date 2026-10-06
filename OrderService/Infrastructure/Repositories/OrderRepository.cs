using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entity;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();  
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
                return order;

            return null;
        }

        public async Task<(List<Order>, int)> GetPagedAsync(int page, int pageSize)
        {
            var total = await _context.Orders.CountAsync();

            pageSize = Math.Min(pageSize, 50);

            var items = await _context.Orders
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
