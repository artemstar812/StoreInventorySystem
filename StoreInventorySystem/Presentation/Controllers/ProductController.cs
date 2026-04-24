using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreInventorySystem.Application.DTOs;
using StoreInventorySystem.Application.DTOs.Product;
using StoreInventorySystem.Application.Services;

namespace StoreInventorySystem.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(int page = 1, int pageSize = 20)
        {
            var result = await _productService.GetProducts(page, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ProductDto>>> Search(string query, int page = 1, int pageSize = 20)
        {
            var result = await _productService.Search(query, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ProductStatsDto>> GetStats()
        {
            var stats = await _productService.GetStats();

            return Ok(stats);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto product)
        {
            var created = await _productService.AddProduct(product);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateProductDto updatedProduct)
        {
            await _productService.UpdateProduct(id, updatedProduct);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);

            return NoContent();
        }
    }
}
