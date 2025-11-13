using ApiUsers.Data;
using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsers.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.IdDeptNavigation)
            .Include(u => u.IdProfileNavigation)
            .FirstOrDefaultAsync(u => u.Email == email);
    }


    public async Task<User?> FindAdminByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.IdDeptNavigation)
            .Include(u => u.IdProfileNavigation)
            .FirstOrDefaultAsync(u => u.Email == email);
    }


}
