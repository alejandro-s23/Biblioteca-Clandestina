using Library.Models;

namespace Library.Data.Interfaces;

public interface IBookRentRepository : IBaseRepository<BookRent>
{
    Task<IEnumerable<BookRent>> GetActiveRents(int size);
    Task<BookRent?> GetActiveRentAsync(Guid userId);
    Task<int> GetActiveRentsCountAsync();
    Task<IEnumerable<BookRent>> GetRentsByBook(Guid bookId);
    Task<IEnumerable<BookRent>> GetActiveRentsByBook(Guid bookId);

}