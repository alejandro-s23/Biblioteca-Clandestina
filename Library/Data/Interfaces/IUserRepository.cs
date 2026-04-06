using Library.Models;

namespace Library.Data.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserAsync(string email, string password);
    Task<List<User>> GetPendingRegistrationsListAsync(int size);
}