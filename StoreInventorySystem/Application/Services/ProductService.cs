using InventorySystem.Application.Interfaces;
using StackExchange.Redis;
using StoreInventorySystem.Application.Common;
using StoreInventorySystem.Application.DTOs;
using StoreInventorySystem.Application.DTOs.Product;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Application.Mappers;
using UserService.Grpc;
using static UserService.Grpc.UsersGrpc;

namespace StoreInventorySystem.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProcessedMessagesRepository _processedMessagesRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICacheService _cache;
        private readonly UsersGrpcClient _usersClient;

        public ProductService(IProductRepository productRepository, IProcessedMessagesRepository processedMessagesRep,
            IUnitOfWork unitOfWOrk, ICacheService cache, UsersGrpcClient usersGrpc)
        {
            _productRepository = productRepository;
            _processedMessagesRepository = processedMessagesRep;
            _unitOfWork = unitOfWOrk;
            _cache = cache;
            _usersClient = usersGrpc;
        }

        public async Task<PagedResult<ProductDto>> GetProducts(int page, int pageSize)
        {
            var key = CacheKeys.Products(page, pageSize);

            var cached = await _cache.GetAsync<PagedResult<ProductDto>>(key);

            if (cached != null)
                return cached;

            var (products, total) = await _productRepository.GetPagedAsync(page, pageSize);
            var productDtos = new List<ProductDto>();

            foreach(var product in products)
            {
                var user = await _usersClient.GetUserByIdAsync(new GetUserRequest
                {
                    Id = product.CreatedByUserId
                });

                var dto = ProductMapper.ToDto(product, user.Username);

                productDtos.Add(dto);
            }

            var result = new PagedResult<ProductDto>
            {
                Items = productDtos,
                TotalCount = total
            };

            await _cache.SetAsync(key, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var key = CacheKeys.Product(id);

            var cached = await _cache.GetAsync<ProductDto>(key);

            if (cached != null)
                return cached;

            var product = await _productRepository.GetByIdAsync(id);

            if (product != null)
            {
                var user = await _usersClient.GetUserByIdAsync(new GetUserRequest
                {
                    Id = product.CreatedByUserId
                });

                var dto = ProductMapper.ToDto(product, user.Username);

                await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(5));

                return dto;
            }

            return null;
        }

        public async Task<PagedResult<ProductDto>> Search(string query, int page, int pageSize)
        {
            var (products, total) = await _productRepository.Search(query, page, pageSize);

            var productDtos = new List<ProductDto>();

            foreach(var product in products)
            {
                var user = await _usersClient.GetUserByIdAsync(new GetUserRequest
                {
                    Id = product.CreatedByUserId,
                });

                productDtos.Add(ProductMapper.ToDto(product, user.Username));
            }

            return new PagedResult<ProductDto>
            {
                Items = productDtos,
                TotalCount = total
            };
        }

        public async Task<ProductDto> AddProduct(CreateProductDto dto, int userId)
        {
            var product = ProductMapper.ToEntity(dto);
            product.CreatedByUserId = userId;

            await _productRepository.AddAsync(product);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20)
            );

            var user = await _usersClient.GetUserByIdAsync(new GetUserRequest
            {
                Id = userId
            });

            return ProductMapper.ToDto(product, user.Username);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(id)
            );
        }

        public async Task UpdateProduct(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception($"Not found product with id {id} while updating");

            ProductMapper.UpdateEntity(product, dto);

            await _productRepository.UpdateAsync(id, product);

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

            var result = await _productRepository.GetStats();

            await _cache.SetAsync(key, result, TimeSpan.FromMinutes(10));

            return result;
        }

        private async Task InvalidateProductKeys(params string[] keys)
        {
            await Task.WhenAll(keys.Select(_cache.RemoveAsync));
        }

        public async Task AddProductAmount(int id, int amount)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");
                
            product.Amount += amount;

            await _productRepository.UpdateAsync(id, product);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(id)
            );
        }
        
        public async Task DecreaseProductAmount(int id, int amount)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");
                
            product.Amount -= amount;

            await _productRepository.UpdateAsync(id, product);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(id)
            );
        }

        public async Task HandleOrderFailedAsync(Guid eventId, int productId, int quantity, CancellationToken cancellationToken)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                var alreadyProcessed = await _processedMessagesRepository.ExistsAsync(eventId, ct);

                if (alreadyProcessed)
                    return;

                await AddProductAmount(productId, quantity);

                await _processedMessagesRepository.AddAsync(new InventorySystem.Domain.Entities.ProcessedMessage
                {
                    EventId = eventId,
                    ProcessedAt = DateTime.UtcNow
                });

            }, cancellationToken);

            await InvalidateProductKeys(
                CacheKeys.Stats,
                CacheKeys.Products(1, 20),
                CacheKeys.Product(productId)
            );
        }
    }
}
