using Microsoft.AspNetCore.Mvc;
using Order.API.Repositories;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository repository,
            ILogger<OrderController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Order>>> GetOrders()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Models.Order>>> GetOrdersByCustomer(int customerId)
        {
            try
            {
                var products = await _repository.GetByCustomerIdAsync(customerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Orders for customer: {customerId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Order>> GetOrder(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Order with ID {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Order with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Models.Order>> CreateOrder(Models.Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _repository.CreateAsync(order);
                return CreatedAtAction(nameof(CreateOrder), new { id = created?.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Order>> UpdateProduct(int id, Models.Order order)
        {
            try
            {
                if (id != order.Id)
                    return BadRequest("ID mismatch");

                if (!await _repository.ExistsAsync(id))
                    return NotFound($"Order with ID {id} not found");

                var updated = await _repository.UpdateAsync(order);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating Order with ID: {id}");
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
                    return NotFound($"Order with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting Order with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
