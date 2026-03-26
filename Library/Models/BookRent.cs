using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace Library.Models;

public class BookRent
{
    [Required]
    public int Id { get; set; }
    
    [Required] public int BookId { get; set; }
    public virtual Book? Book { get; set; }
    
    [Required] public Guid ClientId { get; set; }
    public virtual Client? Client { get; set; }
    [NotMapped] public int RentTimeDays { get; set; }
    [Required] public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }

}