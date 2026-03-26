using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models;

public class Client
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string? City { get; set; }
    [Required] public string? District { get; set; }
    [Required] public string? Address { get; set; }
    [Required] public string? Phone { get; set; }
    [Required] public string? Registration { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? Password { get; set; }
    public bool IsApproved { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    public bool IsLocked { get; set; } = false;
    
}