using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users
                        .FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

   
}
