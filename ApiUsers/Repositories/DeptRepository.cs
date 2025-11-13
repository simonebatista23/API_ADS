using ApiUsers.Data;
using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsers.Repositories
{
    public class DeptRepository : IDeptRepository
    {
        private readonly AppDbContext _context;

        public DeptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dept>> GetAllAsync()
        {
            return await _context.Depts.ToListAsync();
        }

        public async Task<Dept> GetByIdAsync(int id)
        {
            return await _context.Depts.FindAsync(id);
        }

        public async Task<IEnumerable<Dept>> GetActiveDeptsAsync()
        {
            return await _context.Depts
                .Where(d => d.AcceptTicket)
                .ToListAsync();
        }

        public async Task<Dept> CreateAsync(Dept dept)
        {
            _context.Depts.Add(dept);
            await _context.SaveChangesAsync();
            return dept;
        }

        public async Task UpdateAsync(Dept dept)
        {
            _context.Entry(dept).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept != null)
            {
                _context.Depts.Remove(dept);
                await _context.SaveChangesAsync();
            }
        }
    }
}
