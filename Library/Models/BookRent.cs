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


    [Required] public DateTime RentDate { get; set; }

    [NotMapped]
    public DateTime ExpectedReturnDate
    {
        get => RentDate.AddDays(14).Date;
        set;
    }
    public DateTime? ReturnDate { get; set; }
    [NotMapped]
    public int RentTimeDays
    {
        get
        {
            if (ReturnDate == null)
                return (int)(Math.Ceiling(DateTime.Now.Date.Subtract(RentDate).TotalDays));
            return (int)(Math.Ceiling(ReturnDate.GetValueOrDefault().Subtract(RentDate).TotalDays));
        }
        set;
    }
}