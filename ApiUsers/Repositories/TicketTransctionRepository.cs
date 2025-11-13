using ApiUsers.Data;
using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;


namespace ApiUsers.Repositories
{
    public class TicketTransctionRepository : ITicketTransctionRepository
    {
        private readonly AppDbContext _context;

        public TicketTransctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketsTransction>> GetAllAsync()
        {
            return await _context.TicketsTransctions.ToListAsync();
        }

        public async Task<TicketsTransction> GetByIdAsync(Guid id)
        {
            return await _context.TicketsTransctions.FindAsync(id);
        }

        public async Task<TicketsTransction> CreateAsync(TicketsTransction ticketsTransction)
        {
            _context.TicketsTransctions.Add(ticketsTransction);
            await _context.SaveChangesAsync();
            return ticketsTransction;
        }

        public async Task UpdateAsync(TicketsTransction ticketsTransction)
        {
            _context.Entry(ticketsTransction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ticketsTransction = await _context.TicketsTransctions.FindAsync(id);
            if (ticketsTransction != null)
            {
                _context.TicketsTransctions.Remove(ticketsTransction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
