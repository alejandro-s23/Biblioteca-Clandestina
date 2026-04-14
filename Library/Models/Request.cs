using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Models.Enums;

namespace Library.Models;

public sealed class Request
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }
    [Required]
    public RequestTypeEnum Type { get; set; }
    [Required]
    public Dictionary<string,object?> Body { get; set; } = new Dictionary<string, object?>();
    public RequestStatusEnum Status { get; set; } = RequestStatusEnum.PENDING;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = null;

}