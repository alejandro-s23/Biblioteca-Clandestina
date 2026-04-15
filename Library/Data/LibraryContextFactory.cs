using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Data;

public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<LibraryContext>();
        optionBuilder.UseSqlServer(
            "Server=localhost,1433;Database=Library;User Id=developer;Password=SenhaSegura123654;TrustServerCertificate=True");
        return new LibraryContext(optionBuilder.Options);
    }
}