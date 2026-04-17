using StoreInventorySystem.Application.DTOs.Product;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public static List<ProductDto> ToDtoList(List<Product> products)
        {
            return products.Select(ToDto).ToList();
        }

        public static Product ToEntity(CreateProductDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
            };
        }

        public static void UpdateEntity(Product product, UpdateProductDto dto)
        {
            product.Name = dto.Name;
            product.Price = dto.Price;
        }
    }
}
