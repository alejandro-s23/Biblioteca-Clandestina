using Library.Models.DTO;

namespace Library.Models.ViewModel;

public class AdminDashboardViewModel
{
    // Resumo de Aluguéis
    public IEnumerable<BookRent>? ActiveRents { get; set; }
    public int TotalActiveRents { get; set; }

    // Resumo de Solicitações
    public IEnumerable<RegisterRequestDTO>? PendingRequests { get; set; }
    public int TotalPendingRequests { get; set; }

    // Métrica Relevante: Acervo e Disponibilidade
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public double AvailabilityRate => TotalBooks > 0 ? (double)AvailableBooks / TotalBooks * 100 : 0;
}