using System.Reflection;
using Library.Data;
using Library.Data.Interfaces;
using Library.Models;
using Library.Models.Enums;
using Library.Models.ViewModel.DTO;
using Library.Services.Interfaces;

namespace Library.Services;

public class RequestService(IRequestRepository requestRepository) : IRequestService
{
    public async Task<bool> CreateRequestAsync(Guid userId, RequestBodyObj body)
    {
        var request = new Request()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = body.Type,
            Body = body.ToBodyRequest()
        };
        if (!await requestRepository.Add(request))
            return false;
        return true;
    }

    public async Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId)
    {
        return await requestRepository.GetRequestStatusAsync(requestId);
    }
}