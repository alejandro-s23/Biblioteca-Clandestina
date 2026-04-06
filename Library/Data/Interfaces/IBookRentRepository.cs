using Library.Models;

namespace Library.Data.Interfaces;

public interface IBookRentRepository : IBaseRepository<BookRent>
{
    Task<IEnumerable<BookRent>> GetActiveRents(int size);
}