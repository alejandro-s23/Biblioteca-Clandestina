using Library.Models;

namespace Library.Data.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetRentedBooksAsync();
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<IEnumerable<Book>> GetBooksByTitleAsync(string searchTitle);
    Task<bool> AnyBookTenantAsync(Guid clientId);
    Task<IEnumerable<string?>> GetExistingAuthorsAsync();
}