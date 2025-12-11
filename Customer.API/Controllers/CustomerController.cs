using Customer.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            ICustomerRepository repository,
            ILogger<CustomerController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Customer>>> GetCustomers()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Customers");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Customer>> GetCustomer(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Customer with ID {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Order with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Models.Customer>> CreateOrder(Models.Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _repository.CreateAsync(customer);
                return CreatedAtAction(nameof(CreateOrder), new { id = created?.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Customer>> UpdateProduct(int id, Models.Customer customer)
        {
            try
            {
                if (id != customer.Id)
                    return BadRequest("ID mismatch");

                if (!await _repository.ExistsAsync(id))
                    return NotFound($"Customer with ID {id} not found");

                var updated = await _repository.UpdateAsync(customer);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating Customer with ID: {id}");
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
                    return NotFound($"Customer with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting Customer with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }


}
