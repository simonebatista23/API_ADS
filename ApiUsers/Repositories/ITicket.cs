using ApiUsers.Models;

namespace ApiUsers.Repositories
{
    public interface ITicket
    {
        Task<IEnumerable<Tickets>> GetAllAsync();
        Task<Tickets> GetByIdAsync(int id);
        Task<Tickets> CreateAsync(Tickets tickets);
        Task UpdateAsync(Tickets tickets);
        Task DeleteAsync(int id);
        Task<Tickets> CreateWithTransactionAsync(Tickets ticket, TicketsTransction transaction);
        Task<IEnumerable<Tickets>> GetTicketsOpenedByUserAsync(int userId);
        Task<IEnumerable<Tickets>> GetTicketsRespondedByUserAsync(int userId);
        Task<IEnumerable<Tickets>> GetTicketsByDepartmentAsync(int deptId);
        //Task<IEnumerable<Tickets>> GetTicketsByStatusAsync(int statusId, int userId);
        Task<List<Tickets>> GetAllUserTicketsAsync(int userId);
        Task<TicketsTransction?> GetLastTransactionByTicketIdAsync(int ticketId);
    }
}
