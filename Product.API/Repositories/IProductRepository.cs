using System.Collections;

namespace Product.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable> GetAllAsync();
        Task<IEnumerable> GetByCategoryAsync(string category);
        Task<Models.Product?> GetByIdAsync(int id);
        Task<Models.Product?> CreateAsync(Models.Product product);
        Task<Models.Product> UpdateAsync(Models.Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
