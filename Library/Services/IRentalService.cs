using Library.Models;

namespace Library.Services;

public interface IRentalService
{
    Task<(bool Success, string Message)> RentBookAsync(int bookId, Guid clientId);
    Task<(bool Success, string Message)> ReturnBookAsync(int rentId);
    Task<bool> IsBookAvailableAsync(int bookId);
    bool UpdateRentTime(BookRent? bookRent);
}