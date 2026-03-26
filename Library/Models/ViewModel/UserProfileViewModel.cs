namespace Library.Models.ViewModel;

public class UserProfileViewModel
{
    // Dados Editáveis
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }

    // Dados de Leitura (Fixos)
    public string? Registration { get; set; }

    // Informação do Aluguel Ativo
    public Book? ActiveBook { get; set; }
    public int RentTimeDays { get; set; }
}