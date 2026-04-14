using Library.Models;
using Library.Models.ViewModel.DTO;

namespace Library.Services.Interfaces;

public interface IUserService
{
    Task<(bool Success, string Message)> UpdateProfileAsync(User client);
    Task<(bool success, string message)> HasRentedBookAsync(Guid userId);
    Task<User?> GetUserAsync(string email, string password);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<(bool success, string message)> RegisterAsync(User client);
    Task<IEnumerable<User>> GetUsersAsync(int size = 0);
    Task<List<User>> GetUsersListAsync(int size = 0);
    
    Task<IEnumerable<User>> GetActiveUsersAsync(int size = 0);
    Task<List<User>> GetActiveUsersListAsync(int size = 0);

    Task<(bool success, string message)> ApproveUser(Guid userId);
    Task<IEnumerable<User>> GetAOrderedUsersAsync(string searchString, string sortOrder, string sortField);
}