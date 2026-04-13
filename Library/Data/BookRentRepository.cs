using Library.Data.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class BookRentRepository(LibraryContext context) : BaseRepository<BookRent>(context), IBookRentRepository
{
    public override async Task<BookRent?> GetByIdAsync(Guid id)
    {
        return DbSet
            .Include(bookRent => bookRent.Book)
            .FirstOrDefault(b => b.Id == id);
    }

    public async Task<IEnumerable<BookRent>> GetActiveRents(int size)
    {
        var query = DbSet
            .Include(br => br.Book)
            .Include(br => br.User)
            .Where(r => r.ReturnDate == null);
        return size == 0 ? await query.ToListAsync() : await query.Take(size).ToListAsync();

    }

    public async Task<BookRent?> GetActiveRentAsync(Guid userId)
    {
        return await DbSet
            .OrderByDescending(x => x.RentDate)
            .Include(bookRent => bookRent.Book)
            .FirstOrDefaultAsync(b => b.UserId == userId && b.ReturnDate == null);
    }
}