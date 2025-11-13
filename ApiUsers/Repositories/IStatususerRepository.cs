using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface IStatususerRepository
    {
        Task<IEnumerable<Statususer>> GetAllAsync();
        Task<Statususer> GetByIdAsync(int id);
        Task<IEnumerable<Statususer>> GetActiveAsync();
        Task<Statususer> CreateAsync(Statususer statususer);
        Task UpdateAsync(Statususer statususer);
        Task DeleteAsync(int id);
    }
}
