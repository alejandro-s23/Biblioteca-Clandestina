using System.Security.Principal;

namespace Library.Models.DTO;

public class SimpleUserDTO
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Registration { get; set; }
    public bool Approved { get; set; }
    
}