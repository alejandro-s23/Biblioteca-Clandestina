using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required] [MaxLength(50, ErrorMessage = "O Nome deve conter no máximo 50 caracteres")]
    public string? FirstName { get; set; }
    
    [Required] [MaxLength(70, ErrorMessage = "O Sobronome deve conter no máximo 70 caracteres")]
    public string? LastName { get; set; }
    
    [Required] [MaxLength(60, ErrorMessage = "Nome da cidade deve conter no máximo 60 caracteres")]
    public string? City { get; set; }
    
    [Required] [MaxLength(60, ErrorMessage = "O bairro deve conter no máximo 60 caracteres")]
    public string? District { get; set; }
    
    [Required] [MaxLength(60, ErrorMessage = "O endereço deve conter no máximo 60 caracteres")]
    public string? Address { get; set; }
    
    [Required] [MaxLength(10, ErrorMessage = "o número deve conter no máximo 10 dígitos")]
    public string? AddressNumber { get; set; }

    [MaxLength(50, ErrorMessage = "O complemento deve conter no maximo 50 caracteres")]
    public string? Complement { get; set; } = null;
    
    [Required] [MaxLength(11, ErrorMessage = "Número inválido: Ex.: 53912345678")]
    public string? Phone { get; set; }
    
    [Required] [MaxLength(15, ErrorMessage = "A matrícula deve conter no máximo 10 caracteres")]
    public string? Registration { get; set; }
    
    [Required] [MaxLength(50, ErrorMessage = "O email deve conter no maximo 50 caracteres")]
    public string? Email { get; set; }
    [Required] [MaxLength(11, ErrorMessage = "CPF inválido: Ex.: 12345678901")]
    public string? CPF { get; set; }
    
    [Required] [MaxLength(50, ErrorMessage = "A senha deve conter no máximo 50 caracteres")]
    public string? Password { get; set; }
    public bool IsApproved { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
}