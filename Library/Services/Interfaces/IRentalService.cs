using Library.Models;

namespace Library.Services.Interfaces;

public interface IRentalService
{
    Task<(bool Success, string Message)> RentBookAsync(Guid bookId, Guid clientId);
    Task<(bool Success, string Message)> ReturnBookAsync(Guid bookId);
    Task<BookRent?> GetActiveRentAsync(Guid clientId);
    Task<IEnumerable<BookRent>> GetActiveRents(int size = 0);
    Task<IEnumerable<BookRent>> GetAllRentsAsync();
}