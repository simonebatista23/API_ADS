using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface IStatusTicketRepository
    {
        Task<IEnumerable<StatusTickets>> GetAllAsync();
        Task<StatusTickets> GetByIdAsync(int id);
        Task<IEnumerable<StatusTickets>> GetActiveAsync();
        Task<StatusTickets> CreateAsync(StatusTickets statustickets);
        Task UpdateAsync(StatusTickets statustickets);
        Task DeleteAsync(int id);
    }
}
