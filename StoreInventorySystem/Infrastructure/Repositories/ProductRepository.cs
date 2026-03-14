using Microsoft.EntityFrameworkCore;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product != null)
                return product;

            return null;
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            _context.Products.Update(updatedProduct);

            await _context.SaveChangesAsync();
        }
    }
}
