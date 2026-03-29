using Library.Models;

namespace Library.Services;

public interface IRentalService
{
    Task<(bool Success, string Message)> RentBookAsync(Guid bookId, Guid clientId);
    Task<(bool Success, string Message)> ReturnBookAsync(Guid bookId);
    Task<bool> IsBookAvailableAsync(Guid bookId);
    bool UpdateRentTime(BookRent? bookRent);
}