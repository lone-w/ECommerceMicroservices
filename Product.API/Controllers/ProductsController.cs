using Microsoft.AspNetCore.Mvc;
using Product.API.Repositories;
using System.Collections;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductRepository repository,
            ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Product>>> GetProducts()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Models.Product>>> GetProductsByCategory(string category)
        {
            try
            {
                var products = await _repository.GetByCategoryAsync(category);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving products for category: {category}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Product>> GetProduct(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Models.Product>> CreateProduct(Models.Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _repository.CreateAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = created?.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Product>> UpdateProduct(int id, Models.Product product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("ID mismatch");

                if (!await _repository.ExistsAsync(id))
                    return NotFound($"Product with ID {id} not found");

                var updated = await _repository.UpdateAsync(product);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                if (!result)
                    return NotFound($"Product with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
