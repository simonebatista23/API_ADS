using ApiUsers.Data;
using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsers.Repositories
{
    public class StatusTicketRepository : IStatusTicketRepository
    {

        private readonly AppDbContext _context;
        public StatusTicketRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<StatusTickets>> GetAllAsync()
        {
            return await _context.Statustickets.ToListAsync();
        }

        public async Task<StatusTickets> GetByIdAsync(int id)
        {
            return await _context.Statustickets.FindAsync(id);
        }

        public async Task<IEnumerable<StatusTickets>> GetActiveAsync()
        {
            return await _context.Statustickets
                .Where(s => s.Desc == "ATIVO")
                .ToListAsync();
        }

        public async Task<StatusTickets> CreateAsync(StatusTickets statusticket)
        {
            _context.Statustickets.Add(statusticket);
            await _context.SaveChangesAsync();
            return statusticket;
        }

        public async Task UpdateAsync(StatusTickets statusticket)
        {
            _context.Entry(statusticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var statusticket = await _context.StatusTickets.FindAsync(id);
            if (statusticket != null)
            {
                _context.StatusTickets.Remove(statusticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
