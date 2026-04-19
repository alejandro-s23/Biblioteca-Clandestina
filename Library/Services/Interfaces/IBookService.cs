using Library.Models;

namespace Library.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetOrderedBooksAsync(string searchString, string sortOrder, string sortField);
    Task<int> CountAvailableAsync();
    Task<int> CountAsync();
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<IEnumerable<string?>> GetAuthorsAsync();
    Task<bool> AddBookAsync(Book book);
    Task<(bool success, string message)> Delete(Guid id);
}