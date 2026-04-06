using Library.Data.Interfaces;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class BookService (IBookRepository bookRepository): IBookService
{
    public async Task<IEnumerable<Book>> GetOrderedBooksAsync(string searchString,string sortOrder, string sortField)
    {
        IEnumerable<Book> books;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            books = await bookRepository.GetBooksByTitleAsync(searchString);
        }
        else
        {
            books = await bookRepository.GetAllAsync();
        }
        books = sortField switch
        {
            "Author" => sortOrder == "desc" ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
            "Available" => sortOrder == "desc" ? books.OrderByDescending(b => b.Avaliable) : books.OrderBy(b => b.Avaliable),
            _ => sortOrder == "desc" ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
        };
        return books;
    }

    public async Task<int> CountAvailableAsync()
    {
        var available =  await bookRepository.GetAvailableBooksAsync();
        return available.Count();
    }

    public async Task<int> CountAsync()
    {
        return await bookRepository.CountAsync();
    }

    public async Task<IEnumerable<string?>> GetAuthorsAsync()
    {
        return await bookRepository.GetExistingAuthorsAsync();
    }

    public async Task<bool> AddBookAsync(Book book)
    {
        if (await bookRepository.Add(book))
        {
            return true;
        }

        return false;
    }
}