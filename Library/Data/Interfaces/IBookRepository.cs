using Library.Models;

namespace Library.Data.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetRentedBooks();
    Task<IEnumerable<Book>> GetAvailableBooks();
    Task<IEnumerable<Book>> GetBooksByTitle(string searchTitle);
    Task<bool> AnyBookTenantAsync(Guid clientId);
}