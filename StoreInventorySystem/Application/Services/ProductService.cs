using StackExchange.Redis;
using StoreInventorySystem.Application.Common;
using StoreInventorySystem.Application.DTOs;
using StoreInventorySystem.Application.DTOs.Product;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Application.Mappers;

namespace StoreInventorySystem.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;

        public ProductService(IProductRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<PagedResult<ProductDto>> GetProducts(int page, int pageSize)
        {
            var key = CacheKeys.Products(page, pageSize);

            var cached = await _cache.GetAsync<PagedResult<ProductDto>>(key);

            if (cached != null)
                return cached;

            var (products, total) = await _repository.GetPagedAsync(page, pageSize);

            var result = new PagedResult<ProductDto>
            {
                Items = ProductMapper.ToDtoList(products),
                TotalCount = total
            };

            await _cache.SetAsync(key, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();

            return ProductMapper.ToDtoList(products);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var key = CacheKeys.Product(id);

            var cached = await _cache.GetAsync<ProductDto>(key);

            if (cached != null)
                return cached;

            var product = await _repository.GetByIdAsync(id);

            if (product != null)
            {
                var dto = ProductMapper.ToDto(product);

                await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(5));

                return dto;
            }

            return null;
        }

        public async Task<PagedResult<ProductDto>> Search(string query, int page, int pageSize)
        {
            var (products, total) = await _repository.Search(query, page, pageSize);

            return new PagedResult<ProductDto>
            {
                Items = ProductMapper.ToDtoList(products),
                TotalCount = total
            };
        }

        public async Task<ProductDto> AddProduct(CreateProductDto dto)
        {
            var product = ProductMapper.ToEntity(dto);

            await _repository.AddAsync(product);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20)
            );

            return ProductMapper.ToDto(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _repository.DeleteAsync(id);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(id)
            );
        }

        public async Task UpdateProduct(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                throw new Exception($"Not found product with id {id} while updating");

            ProductMapper.UpdateEntity(product, dto);

            await _repository.UpdateAsync(id, product);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(id)
            );
        }

        public async Task<ProductStatsDto> GetStats()
        {
            var key = CacheKeys.Stats;

            var cached = await _cache.GetAsync<ProductStatsDto>(key);

            if (cached != null)
                return cached;

            var result = await _repository.GetStats();

            await _cache.SetAsync(key, result, TimeSpan.FromMinutes(10));

            return result;
        }

        private async Task InvalidateProductKeys(params string[] keys)
        {
            await Task.WhenAll(keys.Select(k => _cache.RemoveAsync(k)));
        }
    }
}
