using Customer.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Customer.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<ICustomerRepository> _logger;

        public CustomerRepository(CustomerDbContext context, ILogger<ICustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            return await _context.Customers
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<Models.Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Models.Customer?> CreateAsync(Models.Customer customer)
        {
            customer.CreatedDate = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Customer created with ID: {customer.Id}");
            return customer;
        }

        public async Task<Models.Customer?> UpdateAsync(Models.Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"customer updated with ID: {customer.Id}");
            return customer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Customers.FindAsync(id);
            if (order == null) return false;

            _context.Customers.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"customer deleted with ID: {id}");
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(o => o.Id == id);
        }
    }
}

