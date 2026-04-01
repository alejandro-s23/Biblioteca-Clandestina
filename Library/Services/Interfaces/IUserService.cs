using Library.Models;

namespace Library.Services.Interfaces;

public interface IUserService
{
    Task<(bool Success, string Message)> UpdateProfileAsync(Client client);
    Task<bool> IsAdminAsync(Guid id);
    Task<bool> HasRentedBookAsync(Guid userId);
}