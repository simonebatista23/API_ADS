using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface IProfileRepository
    {
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile> GetByIdAsync(int id);
        Task<Profile> CreateAsync(Profile profile);
        Task UpdateAsync(Profile profile);
        Task DeleteAsync(int id);
    }
}
