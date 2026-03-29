using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace Library.Models;

public sealed class BookRent
{
    [Required] [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required] public Guid BookId { get; set; }
    public Book? Book { get; set; }
    
    [Required] public Guid ClientId { get; set; }
    public Client? Client { get; set; }
    [NotMapped] public int RentTimeDays { get; set; }
    [Required] public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }

}