using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface ITicketTransctionRepository
    {
        Task<IEnumerable<TicketsTransction>> GetAllAsync();
        Task<TicketsTransction> GetByIdAsync(Guid id);
        Task<TicketsTransction> CreateAsync(TicketsTransction ticketsTransction);
        Task UpdateAsync(TicketsTransction ticketsTransction);
        Task DeleteAsync(Guid id);
    }
}
