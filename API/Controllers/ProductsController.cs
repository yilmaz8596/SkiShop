using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsRepository productsRepository) : ControllerBase
    {
        private readonly IProductsRepository _productsRepository = productsRepository;

        [HttpGet]
        public async Task<IActionResult> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _productsRepository.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productsRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            _productsRepository.AddProduct(product);
            await _productsRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!_productsRepository.ProductExists(id))
                return NotFound();
            _productsRepository.UpdateProduct(product);
            if (!await _productsRepository.SaveChangesAsync())
                return StatusCode(500, "A problem happened while handling your request.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            _productsRepository.DeleteProduct(product);
            if (!await _productsRepository.SaveChangesAsync())
                return StatusCode(500, "A problem happened while handling your request.");
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var brands = await _productsRepository.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var types = await _productsRepository.GetTypesAsync();
            return Ok(types);
        }
    }
}
