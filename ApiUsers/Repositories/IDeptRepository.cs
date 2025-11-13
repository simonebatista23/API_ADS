using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface IDeptRepository
    {
        Task<IEnumerable<Dept>> GetAllAsync();
        Task<Dept> GetByIdAsync(int id);
        Task<IEnumerable<Dept>> GetActiveDeptsAsync();
        Task<Dept> CreateAsync(Dept dept);
        Task UpdateAsync(Dept dept);
        Task DeleteAsync(int id);
    }
}
