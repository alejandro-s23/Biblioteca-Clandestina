using Library.Models;
using Library.Models.Enums;

namespace Library.Data.Interfaces;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId);
    Task<IEnumerable<Request>> GetPendingRequestsByUserAsync(Guid userId);
    Task<bool> HasUserPendingRequestAsync(Guid userId, RequestTypeEnum type);
    Task<IEnumerable<Request>> GetRequestsByType(RequestTypeEnum type);
    Task<IEnumerable<Request>> GetActiveRequestsByType(RequestTypeEnum type);
    Task<IEnumerable<Request>> GetActiveRequests();
}