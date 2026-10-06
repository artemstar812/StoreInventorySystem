using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entity;

namespace OrderService.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    }
}
