using System.Text.Json;
using Library.Data.Interfaces;
using Library.Models;
using Library.Models.Enums;
using Library.Services.Interfaces;

namespace Library.Services;

public class RequestService(
    ILogger<RequestService> logger,
    IRequestRepository requestRepository, 
    IBookRepository bookRepository, 
    IBookRentRepository bookRentRepository,
    IUserService userService,
    IRentalService rentalService
    ) : IRequestService
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
                //Validando a request
                if (signup == null)
                    logger.LogError($"Request {model.Id} was not found");
                //Validando o obj User
                if (model.User == null)
                    logger.LogError($"User was not found");
                //Atribuindo valores ao objBody
                return signup;
            case RequestTypeEnum.RETURNS:
                obj = MapTo<ReturnsRequestBody>(model.Body);
                var returns = obj as ReturnsRequestBody;
                returns.Book = await bookRepository.GetByIdAsync(returns.BookId);
                returns.BookRent = await bookRentRepository.GetByIdAsync(returns.RentId);
                returns.BookRent?.RentTimeDays =
                    (int)(Math.Ceiling(model.CreatedAt.Subtract(returns.RentDate).TotalDays));
                return returns;
            default:
                throw new ArgumentOutOfRangeException(nameof(model.Type));
        }
    }
    
    public Task<IEnumerable<Request>> GetActiveRequests()
    {
        return requestRepository.GetActiveRequests();
    }

    public async Task<(bool success, string message)> ApproveAsync(Guid id)
    {

        var result = await ApproveAsync(await requestRepository.GetByIdAsync(id));
        if (!result.success)
        {
            logger.LogError($"Request {id} was not found");
            return (false, "Request was not found");
        }
        return result;
    }

    public async Task<(bool success, string message)> ApproveAsync(Request? request)
    {
        
        if (request == null) return (false, "Request is null");

        request.Status = RequestStatusEnum.APPROVED;
        request.UpdatedAt = DateTime.Now;
        switch (request.Type)
        {
            case RequestTypeEnum.REGISTER:
                return await userService.ApproveUser(request.UserId);
            case RequestTypeEnum.RETURNS:
                var requestBodyReturn = await ResolveRequestBody(request);
                return await rentalService.ReturnBookAsync(((ReturnsRequestBody)requestBodyReturn).BookId);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task<int> GetPendingRequestsCountAsync(RequestTypeEnum type)
    {
        return await requestRepository.GetPendingRequestsCountAsync(type);
    }

    public async Task<(bool success, string message)> DeleteByBookIdAsync(Guid bookId)
    {
        //Verificando se o livro existe
        if (await bookRepository.ExistsAsync(bookId))
            return (false, "Livro não encontrado!");
        //Executando query de delete
        var result = await requestRepository
            .DeleteRequestsByObjIdAsync(bookId,RequestTypeEnum.RETURNS);
        
        return result;
    }

    private T MapTo<T>(Dictionary<string, object?> dict) where T : RequestBodyObj
    {
        var json = JsonSerializer.Serialize(dict);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}