using Grpc.Core;
using InventoryService.Grpc;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Application.Services;

namespace InventorySystem.Infrastructure.Grpc
{
    public class InventoryGrpcService : InventoryGrpc.InventoryGrpcBase
    {
        private readonly IProductRepository _repository;
        private readonly ProductService _productService;

        public InventoryGrpcService(IProductRepository repository, ProductService productService) 
        {
            _repository = repository;
            _productService = productService;
        }

        public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);

            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = (double)product.Price,
                Amount = product.Amount
            };
        }

        public override async Task<ReserveProductResponse> ReserveProduct(ReserveProductRequest request, ServerCallContext context)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);

            if(product is null)
            {
                return new ReserveProductResponse
                {
                    Success = false,
                    Message = "Product not found",
                    Price = 0
                };
            }

            if(product.Amount >= request.Quantity)
            {
                await _productService.DecreaseProductAmount(request.ProductId, request.Quantity);

                double price = (double)(product.Price * request.Quantity);

                return new ReserveProductResponse
                {
                    Success = true,
                    Message = "Success",
                    Price = price
                };
            }
            else
            {
                return new ReserveProductResponse
                {
                    Success = false,
                    Message = "Not enough quantity of product",
                    Price = 0
                };
            }
        }
    }
}
