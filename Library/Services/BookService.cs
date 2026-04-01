using Library.Data.Interfaces;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class BookService (IBookRepository bookRepository): IBookService
{
    public IEnumerable<Book> GetOrderedBooks(string searchString,string sortOrder, string sortField)
    {
        IEnumerable<Book> books;
        if (string.IsNullOrWhiteSpace(searchString))
        {
            books = bookRepository.GetBooksByTitle(searchString);
        }
        else
        {
            books = bookRepository.GetA
        }
        books = sortField switch
        {
            "Author" => sortOrder == "desc" ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
            "Available" => sortOrder == "desc" ? books.OrderByDescending(b => b.Avaliable) : books.OrderBy(b => b.Avaliable),
            _ => sortOrder == "desc" ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
        };
        return books;
    }
}