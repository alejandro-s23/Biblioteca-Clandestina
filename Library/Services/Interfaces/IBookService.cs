using Library.Models;

namespace Library.Services.Interfaces;

public interface IBookService
{
    IEnumerable<Book> GetOrderedBooks(string searchString, string sortOrder, string sortField);
}