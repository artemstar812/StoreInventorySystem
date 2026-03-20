using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> Search(string query);
        Task AddAsync(Product product);
        Task UpdateAsync(int id, Product updatedProduct);
        Task DeleteAsync(int id);
    }
}
