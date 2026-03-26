using System.ComponentModel.DataAnnotations;

namespace Library.Models.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "O selo de identificação (E-mail) é obrigatório.")]
    [EmailAddress(ErrorMessage = "Este formato de correio não é reconhecido nos registros.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A chave de acesso é necessária.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}