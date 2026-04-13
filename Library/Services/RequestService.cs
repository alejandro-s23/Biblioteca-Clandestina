using System.Reflection;
using System.Text.Json;
using Library.Data;
using Library.Data.Interfaces;
using Library.Models;
using Library.Models.DTO;
using Library.Models.Enums;
using Library.Models.ViewModel.DTO;
using Library.Services.Interfaces;

namespace Library.Services;

public class RequestService(IRequestRepository requestRepository, IBookRepository bookRepository, IBookRentRepository bookRentRepository) : IRequestService
{
    public async Task<(bool success, string message)> CreateRequestAsync(Guid userId, RequestTypeEnum type ,RequestBodyObj body)
    {
        if (await requestRepository.HasUserPendingRequestAsync(userId, type))
        {
            return (false, "Request is already pending for this user");
        }
        var request = new Request()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Body = body.ToDict()
        };
        if (!await requestRepository.Add(request))
            return (false, "Error creating request");
        return (true, string.Empty);
    }

    public async Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId)
    {
        return await requestRepository.GetRequestStatusAsync(requestId);
    }

    public async Task<bool> HasRequestPendingAsync(Guid userId, RequestTypeEnum type)
    {
        return await requestRepository.HasUserPendingRequestAsync(userId, type);
    }

    public async Task<IEnumerable<Request>> GetActiveRequestByType(RequestTypeEnum type)
    {
        return await requestRepository.GetActiveRequestsByType(type);
    }

    public async Task<RequestBodyObj> ResolveRequestBody(Request model)
    {
        if (model == null) return null;

        RequestBodyObj obj;
        switch (model.Type)
        {
            case RequestTypeEnum.REGISTER:
                obj = MapTo<RegisterRequestBody>(model.Body);
                var signup = obj as RegisterRequestBody;
                return signup;
            case RequestTypeEnum.RETURNS:
                obj = MapTo<ReturnsRequestBody>(model.Body);
                var returns = obj as ReturnsRequestBody;
                returns.Book = await bookRepository.GetByIdAsync(returns.BookId);
                returns.BookRent = await bookRentRepository.GetByIdAsync(returns.RentId);
                return returns;
            default:
                throw new ArgumentOutOfRangeException(nameof(model.Type));
        }
    }

    public Task<IEnumerable<Request>> GetActiveRequests()
    {
        return requestRepository.GetActiveRequests();
    }

    private T MapTo<T>(Dictionary<string, object?> dict) where T : RequestBodyObj
    {
        var json = JsonSerializer.Serialize(dict);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}