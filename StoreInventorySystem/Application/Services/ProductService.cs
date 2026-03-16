using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Product>> GetProducts()
        {
            return _repository.GetAllAsync();
        }

        public Task<Product?> GetProductById(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Product>> Search(string query)
        {
            return _repository.Search(query);
        }

        public Task AddProduct(Product product)
        {
            return _repository.AddAsync(product);
        }

        public Task DeleteProduct(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task UpdateProduct(Product updatedProduct)
        {
            return _repository.UpdateAsync(updatedProduct);
        }
    }
}
