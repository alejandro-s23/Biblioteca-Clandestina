using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class BookRentRepository(LibraryContext context) : BaseRepository<BookRent>(context), IBookRentRepository
{
    
    public async Task<IEnumerable<BookRent>> GetActiveRents(int size)
    {
        return await DbSet
            .Include(br => br.Book)
            .Include(br => br.User)
            .Where( r => r.ReturnDate == null)
            .Take(size)
            .ToListAsync();
    }
}