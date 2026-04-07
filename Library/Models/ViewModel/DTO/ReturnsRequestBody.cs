using Library.Models.Enums;

namespace Library.Models.ViewModel.DTO;

public class ReturnsRequestBody : RequestBodyObj
{
    public Guid BookId { get; set; }
    public Guid RentId { get; set; }
    public DateTime RentDate  { get; set; }
    public DateTime ExpectedReturnDate
    {
        get => RentDate.Date.AddDays(14);
        set;
    }
    
    public ReturnsRequestBody()
    {
        this.Type = RequestTypeEnum.RETURNS;
    }
    
}