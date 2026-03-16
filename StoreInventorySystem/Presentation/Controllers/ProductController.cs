using Microsoft.AspNetCore.Mvc;
using StoreInventorySystem.Application.Services;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search(string query)
        {
            var products = _productService.Search(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _productService.AddProduct(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product updatedProduct)
        {
            await _productService.UpdateProduct(updatedProduct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);

            return NoContent();
        }
    }
}
