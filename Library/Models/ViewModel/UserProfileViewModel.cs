namespace Library.Models.ViewModel;

public class UserProfileViewModel
{
    // Dados Editáveis
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Number { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }

    // Dados de Leitura (Fixos)
    public string? FullName { get; set; }
    public string? Registration { get; set; }
    public string? Cpf { get; set; }
    public string? HomePage { get; set; }

    // Informação do Aluguel Ativo
    public Book? ActiveBook { get; set; }
    public int RentTimeDays { get; set; }
    public bool HasReturnRequest { get; set; }
}