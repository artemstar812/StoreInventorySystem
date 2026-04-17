using StoreInventorySystem.Application.DTOs.Product;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<(List<Product>, int)> GetPagedAsync(int page, int pageSize);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<(List<Product>, int)> Search(string query, int page, int pageSize);
        Task AddAsync(Product product);
        Task UpdateAsync(int id, Product updatedProduct);
        Task DeleteAsync(int id);
        Task<ProductStatsDto> GetStats();
    }
}
