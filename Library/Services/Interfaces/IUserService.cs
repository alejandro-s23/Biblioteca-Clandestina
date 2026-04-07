using Library.Models;

namespace Library.Services.Interfaces;

public interface IUserService
{
    Task<(bool Success, string Message)> UpdateProfileAsync(User client);
    Task<bool> HasRentedBookAsync(Guid userId);
    Task<User?> GetUserAsync(string email, string password);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<bool> RegisterAsync(User client);
    Task<List<User>> GetUsersAsync(int size = 5);
    Task<bool> ApproveUser(Guid userId);
}