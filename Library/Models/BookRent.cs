using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models;

public sealed class BookRent
{
    [Required] [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public Guid BookId { get; set; }
    public Book? Book { get; set; }
    
    [Required] public Guid UserId { get; set; }
    public User? User { get; set; }
    [NotMapped] public int RentTimeDays { get; set; }
    [Required] public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }

}