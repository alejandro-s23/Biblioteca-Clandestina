using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class BookRepository(LibraryContext context) : BaseRepository<Book>(context),IBookRepository
{
    private readonly LibraryContext _context = context;

    public async Task<IEnumerable<Book>> GetRentedBooksAsync()
    {
        return await _context.Books.Where(b => !b.Avaliable).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _context.Books.Where(b => b.Avaliable).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string searchTitle)
    {
        return await _context.Books
            .Where(b => b.Title != null && b.Title.ToLower().Contains(searchTitle.ToLower()))
            .ToListAsync();
    }

    public async Task<bool> AnyBookTenantAsync(Guid clientId)
    {
        return await _context.Books.AnyAsync(b => b.CurrentRent != null && b.CurrentRent.ClientId == clientId);
    }

    public async Task<IEnumerable<string?>> GetExistingAuthorsAsync()
    {
        return await _context.Books
            .Select(b => b.Author)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
    }
}