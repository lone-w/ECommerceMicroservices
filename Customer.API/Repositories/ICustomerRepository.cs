using System.Collections;

namespace Customer.API.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable> GetAllAsync();
        Task<Models.Customer?> GetByIdAsync(int id);
        Task<Models.Customer?> CreateAsync(Models.Customer order);
        Task<Models.Customer?> UpdateAsync(Models.Customer order);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
