using InventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => p.Name);
            modelBuilder.Entity<ProcessedMessage>().HasKey(m => m.EventId);
        }
    }
}
