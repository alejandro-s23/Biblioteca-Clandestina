using Library.Models;

namespace Library.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetOrderedBooks(string searchString, string sortOrder, string sortField);
}