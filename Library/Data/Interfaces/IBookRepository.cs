using Library.Models;

namespace Library.Data.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetRentedBooks();
    IEnumerable<Book> GetAvailableBooks();
    IEnumerable<Book> GetBooksByTitle(string searchTitle);
    Task<bool> AnyBookTenantAsync(Guid clientId);
}