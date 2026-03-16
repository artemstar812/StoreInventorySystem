using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Application.Services;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Infrastructure.Repositories
{
    public class TestProductRepository : IProductRepository
    {
        private List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Phone", Price = 500 },
            new Product { Id = 2, Name = "Laptop", Price = 1200}
        };

        public Task AddAsync(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;

            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);

            if(product != null) 
                _products.Remove(product);
            
            return Task.CompletedTask;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_products);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<List<Product>> Search(string query)
        {
            return Task.FromResult(_products.Where(p => p.Name.StartsWith(query)).ToList());
        }

        public Task UpdateAsync(Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);

            if(product != null)
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
            }    

            return Task.CompletedTask;
        }
    }
}
