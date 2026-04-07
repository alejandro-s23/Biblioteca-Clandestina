using Library.Models.Enums;
using Library.Models.ViewModel.DTO;

namespace Library.Services.Interfaces;

public interface IRequestService
{
    Task<bool> CreateRequestAsync(Guid userId, RequestBodyObj body);
    Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId);
}