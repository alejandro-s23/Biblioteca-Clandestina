using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class BookRepository(LibraryContext context) : IBookRepository
{
    public Task<T?> GetByIdAsync<T>(Guid id) where T : class
    {
        throw new NotImplementedException();
    }

    public Task<List<Book>> GetAllAsync<Book>()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Book entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Add(Book entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Book entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Book> GetRentedBooks()
    {
        return context.Books.Where(b => !b.Avaliable).AsEnumerable();
    }

    public IEnumerable<Book> GetAvailableBooks()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Book> GetBooksByTitle(string searchTitle)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AnyBookTenantAsync(Guid clientId)
    {
        return await context.Books.AnyAsync(b => b.CurrentRent != null && b.CurrentRent.ClientId == clientId);
    }
}