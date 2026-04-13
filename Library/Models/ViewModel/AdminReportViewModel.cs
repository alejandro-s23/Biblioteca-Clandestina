using Library.Models.DTO;
using Library.Models.ViewModel.DTO;

namespace Library.Models.ViewModel;

public class AdminReportViewModel
{
    public IEnumerable<User>? Users { get; set; } =  new List<User>();
    public IEnumerable<Book> Books { get; set; } =  new List<Book>();
    public IEnumerable<BookRent> Rents { get; set; } =   new List<BookRent>();
    public IEnumerable<ReturnRequestDTO> ReturnsRequests { get; set; } = new List<ReturnRequestDTO>();
    public IEnumerable<RegisterRequestDTO> SignUpRequests { get; set; } = new List<RegisterRequestDTO>();

}