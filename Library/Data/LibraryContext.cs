using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // Seus "Registros" (Tabelas)
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BookRent> BookRents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookRent>().ToTable("BookRents");
            // 1. Configura a relação 1:1 entre Book e BookRent
            modelBuilder.Entity<BookRent>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId);

            // 2. O GRANDE SEGREDO: Índice Único Filtrado
            // Isso garante que um ClientId só apareça UMA VEZ na tabela onde ReturnDate for NULL
            modelBuilder.Entity<BookRent>()
                .HasIndex(br => br.ClientId)
                .IsUnique()
                .HasFilter("[ReturnDate] IS NULL"); 

            base.OnModelCreating(modelBuilder);
        }
    }
}