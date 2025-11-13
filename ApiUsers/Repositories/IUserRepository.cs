using ApiUsers.Dtos;
using ApiUsers.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsers.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);

    // Novo método para login
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindAdminByEmailAsync(string email);
}
