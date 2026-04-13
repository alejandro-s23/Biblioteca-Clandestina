using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class UserRepository (LibraryContext context): BaseRepository<User>(context), IUserRepository
{
    private readonly LibraryContext _context = context;

    public async Task<User?> GetUserAsync(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    public async Task<List<User>> GetPendingRegistrationsListAsync(int size)
    {
        return await DbSet.Where(u => !u.IsApproved)
            .Take(size)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(int size)
    {
        var query = DbSet.Where(u => u.IsApproved);
        return size == 0 ?await  query.ToListAsync() : await query.Take(size).ToListAsync();
    }
    
    public async Task<List<User>> GetActiveUsersListAsync(int size)
    {
        var query = DbSet.Where(u => u.IsApproved);
        return size == 0 ?await  query.ToListAsync() : await query.Take(size).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
    {
        return await DbSet.Where(u =>
                u.FirstName != null && u.LastName != null &&
                (u.FirstName + " " + u.LastName).ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }
}