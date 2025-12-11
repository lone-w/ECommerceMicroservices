using Order.API.Models;
using System.Collections;

namespace Order.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable> GetAllAsync();
        Task<IEnumerable> GetByCustomerIdAsync(int customerId);
        Task<Models.Order?> GetByIdAsync(int id);
        Task<Models.Order?> CreateAsync(Models.Order order);
        Task<Models.Order?> UpdateAsync(Models.Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
