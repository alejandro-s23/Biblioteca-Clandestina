using Library.Models;
using Library.Models.Enums;

namespace Library.Data.Interfaces;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId);
}