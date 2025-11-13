using ApiUsers.Data;
using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsers.Repositories
{
    public class StatususerRepository : IStatususerRepository
    {
        private readonly AppDbContext _context;

        public StatususerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Statususer>> GetAllAsync()
        {
            return await _context.UserStatuses.ToListAsync();
        }

        public async Task<Statususer> GetByIdAsync(int id)
        {
            return await _context.UserStatuses.FindAsync(id);
        }

        public async Task<IEnumerable<Statususer>> GetActiveAsync()
        {
            return await _context.UserStatuses
                .Where(s => s.Desc == "ATIVADO")
                .ToListAsync();
        }

        public async Task<Statususer> CreateAsync(Statususer statususer)
        {
            _context.UserStatuses.Add(statususer);
            await _context.SaveChangesAsync();
            return statususer;
        }

        public async Task UpdateAsync(Statususer statususer)
        {
            _context.Entry(statususer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var statususer = await _context.UserStatuses.FindAsync(id);
            if (statususer != null)
            {
                _context.UserStatuses.Remove(statususer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
