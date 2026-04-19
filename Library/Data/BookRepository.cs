using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class BookRepository(LibraryContext context) : BaseRepository<Book>(context),IBookRepository
{
    private readonly LibraryContext _context = context;

    public override async Task<Book?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(b => b.CurrentRent)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public new async Task<IEnumerable<Book>> GetEntitiesAsync(int size = 0)
    {
        return size > 0
            ? await DbSet
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Added)
                .Take(size).ToListAsync()
            : await DbSet.Where(b => !b.IsDeleted)
                .ToListAsync();
    }
    
    public new async Task<List<Book>> GetEntitiesListAsync(int size = 0)
    {
        return size > 0
            ? await DbSet
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Added)
                .Take(size).ToListAsync()
            : await DbSet.Where(b => !b.IsDeleted)
                .ToListAsync();
    }

    public new async Task<int> CountAsync()
    {
        return await DbSet.Where(b => !b.IsDeleted).CountAsync();
    }

    public new async Task<bool> ExistsAsync(Guid id)
    {
        return await DbSet.Where(b => !b.IsDeleted).AnyAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetRentedBooksAsync()
    {
        return await _context.Books.Where(b => !b.Available && !b.IsDeleted).ToListAsync();
    }

    public async Task<Book?> GetByIdLoadedAsync(Guid id)
    {
        return await DbSet.Include(b => b.CurrentRent).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _context.Books.Where(b => b.Available && !b.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string searchTitle)
    {
        return await _context.Books
            .Where(b => b.Title != null && b.Title.ToLower().Contains(searchTitle.ToLower()) && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> AnyBookTenantAsync(Guid clientId)
    {
        return await _context.Books.AnyAsync(b => b.CurrentRent != null && b.CurrentRent.UserId == clientId && !b.IsDeleted);
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