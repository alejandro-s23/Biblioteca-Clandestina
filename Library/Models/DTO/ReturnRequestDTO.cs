using Library.Models.Enums;

namespace Library.Models.DTO;

public class ReturnRequestDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public RequestTypeEnum Type { get; set; }
    public ReturnsRequestBody? Body { get; set; }
    public RequestStatusEnum Status { get; set; } = RequestStatusEnum.PENDING;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}