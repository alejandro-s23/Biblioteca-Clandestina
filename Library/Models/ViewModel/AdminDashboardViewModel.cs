namespace Library.Models.ViewModel;

public class AdminDashboardViewModel
{
    // Resumo de Aluguéis
    public List<BookRent> ActiveRents { get; set; } = new();
    public int TotalActiveRents => ActiveRents.Count;

    // Resumo de Solicitações
    public List<Client> PendingRequests { get; set; } = new();
    public int TotalPendingRequests => PendingRequests.Count;

    // Métrica Relevante: Acervo e Disponibilidade
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public double AvailabilityRate => TotalBooks > 0 ? (double)AvailableBooks / TotalBooks * 100 : 0;
}