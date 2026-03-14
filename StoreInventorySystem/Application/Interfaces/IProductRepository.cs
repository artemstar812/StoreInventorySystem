using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);  
        Task AddAsync(Product product);
        Task UpdateAsync(Product updatedProduct);
        Task DeleteAsync(int id);
    }
}
