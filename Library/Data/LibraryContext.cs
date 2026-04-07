using System.Text.Json;
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
        public DbSet<User> Users { get; set; }
        public DbSet<BookRent> BookRents { get; set; }
        public DbSet<Request>  Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurando Entidade BookRents
            modelBuilder.Entity<BookRent>().ToTable("BookRents");
            // Configura a relação 1:1 entre Book e BookRent
            modelBuilder.Entity<BookRent>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId);
            // Isso garante que um ClientId só apareça UMA VEZ na tabela onde ReturnDate for NULL
            modelBuilder.Entity<BookRent>()
                .HasIndex(br => br.UserId)
                .IsUnique()
                .HasFilter("[ReturnDate] IS NULL");
            
            //Configurando a Entidade Request
            modelBuilder.Entity<Request>()
                .Property(b => b.Body)
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null)
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}