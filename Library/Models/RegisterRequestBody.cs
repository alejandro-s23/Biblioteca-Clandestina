using Library.Models.DTO;

namespace Library.Models;

public class RegisterRequestBody : RequestBodyObj
{
    public User? User { get; set; }
}