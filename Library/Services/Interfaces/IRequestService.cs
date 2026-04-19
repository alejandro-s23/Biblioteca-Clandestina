using Library.Models;
using Library.Models.Enums;

namespace Library.Services.Interfaces;

public interface IRequestService
{
    Task<(bool success, string message)> CreateRequestAsync(Guid userId, RequestTypeEnum type, RequestBodyObj body);
    Task<RequestStatusEnum> GetRequestStatusAsync(Guid userId);
    Task<bool> HasRequestPendingAsync(Guid userId, RequestTypeEnum type);
    Task<IEnumerable<Request>> GetActiveRequestByType(RequestTypeEnum type);
    Task<RequestBodyObj> ResolveRequestBody(Request model);
    Task<IEnumerable<Request>> GetActiveRequests();
    Task<(bool success, string message)> ApproveAsync(Guid id);
    Task<(bool success, string message)> ApproveAsync(Request request);
    Task<int> GetPendingRequestsCountAsync(RequestTypeEnum type);
    Task<(bool success, string message)> DeleteByBookIdAsync(Guid bookId);


}