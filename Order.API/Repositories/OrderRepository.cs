using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Models;
using System.Collections;

namespace Order.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<IOrderRepository> _logger;

        public OrderRepository(OrderDbContext context, ILogger<IOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Models.Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Models.Order?> CreateAsync(Models.Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order created with ID: {order.Id}");
            return order;
        }

        public async Task<Models.Order?> UpdateAsync(Models.Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order updated with ID: {order.Id}");
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order deleted with ID: {id}");
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }
    }
}
