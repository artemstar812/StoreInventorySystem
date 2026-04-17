using Microsoft.EntityFrameworkCore;
using StoreInventorySystem.Application.DTOs.Product;
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

        public async Task<(List<Product>, int)> GetPagedAsync(int page, int pageSize)
        {
            var total = await _context.Products.CountAsync();

            pageSize = Math.Min(pageSize, 50);

            var items = await _context.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

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

        public async Task<(List<Product>, int)> Search(string query, int page, int pageSize)
        {
            var total = await _context.Products.CountAsync();

            pageSize = Math.Min(pageSize, 50);

            var items = await _context.Products.Where(p => p.Name.StartsWith(query))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product != null)
                return product;

            return null;
        }
        
        public async Task UpdateAsync(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return;

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();
        }

        public async Task<ProductStatsDto> GetStats()
        {
            int totalProducts = await _context.Products.CountAsync();
            decimal averagePrice = await _context.Products.AverageAsync(p => p.Price);
            decimal maxPrice = await _context.Products.MaxAsync(p => p.Price);
            decimal minPrice = await _context.Products.MinAsync(p => p.Price);

            return new ProductStatsDto
            {
                TotalProducts = totalProducts,
                AveragePrice = averagePrice,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };
        }
    }
}
