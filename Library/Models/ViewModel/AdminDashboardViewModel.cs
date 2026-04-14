namespace Library.Models.ViewModel;

public class AdminDashboardViewModel
{
    // Resumo de Aluguéis
    public IEnumerable<BookRent>? ActiveRents { get; set; }
    public int TotalActiveRents
    {
        get
        {
            if (ActiveRents != null) return ActiveRents.Count();
            return 0;
        }
    }

    // Resumo de Solicitações
    public IEnumerable<User>? PendingRequests { get; set; }
    public int TotalPendingRequests
    {
        get
        {
            if (PendingRequests != null) return PendingRequests.Count();
            return 0;
        }
    }

    // Métrica Relevante: Acervo e Disponibilidade
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public double AvailabilityRate => TotalBooks > 0 ? (double)AvailableBooks / TotalBooks * 100 : 0;
}