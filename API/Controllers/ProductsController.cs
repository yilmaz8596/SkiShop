using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using Core.Entities;
using API.RequestHelpers;

namespace API.Controllers
{
    public class ProductsController(IGenericRepository<Product> productsRepository) : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productsRepository = productsRepository;

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return await CreatePageResult(_productsRepository, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            _productsRepository.Add(product);
            await _productsRepository.SaveAllAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!_productsRepository.Exists(id))
                return NotFound();
            _productsRepository.Update(product);
            if (!await _productsRepository.SaveAllAsync())
                return StatusCode(500, "A problem happened while handling your request.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            _productsRepository.Remove(product);
            if (!await _productsRepository.SaveAllAsync())
                return StatusCode(500, "A problem happened while handling your request.");
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            var brands = await _productsRepository.ListAsync(spec);
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            var types = await _productsRepository.ListAsync(spec);
            return Ok(types);
        }
    }
}
