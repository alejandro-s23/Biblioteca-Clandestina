using Library.Data.Interfaces;
using Library.Models;
using Library.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class RequestRepository(LibraryContext context) : 
    BaseRepository<Request>(context), IRequestRepository
{
    public async Task<RequestStatusEnum> GetRequestStatusAsync(Guid requestId)
    {
        return await DbSet.Where(r => r.Id == requestId).Select(request => request.Status).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Request>> GetPendingRequestsByUserAsync(Guid userId)
    {
        return await DbSet.Where(r => r.UserId == userId && r.Status == RequestStatusEnum.PENDING).ToListAsync();
    }

    public async Task<bool> HasUserPendingRequestAsync(Guid userId, RequestTypeEnum type)
    {
        return await DbSet.AnyAsync(r => r.UserId == userId 
                                         && r.Status == RequestStatusEnum.PENDING 
                                         &&  r.Type == type);
    }

    public async Task<int> GetPendingRequestsCountAsync(RequestTypeEnum type)
    {
        return await DbSet
            .Where(r => r.Status == RequestStatusEnum.PENDING 
                        && r.Type == type).CountAsync();
    }

    public async Task<IEnumerable<Request>> GetRequestsByObjIdAsync(Guid objId, RequestTypeEnum type)
    {
        return type switch
        {
            RequestTypeEnum.REGISTER => await DbSet.Where(r => r.UserId == objId).ToListAsync(),
            RequestTypeEnum.RETURNS => await DbSet
                .Where(r => (r.Body["BookId"] is Guid ? (Guid)r.Body["BookId"] : Guid.Empty) == objId).ToListAsync(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public async Task<(bool success, string message)> DeleteRequestsByObjIdAsync(Guid objId, RequestTypeEnum type)
    {
        return type switch
        {
            RequestTypeEnum.REGISTER => await DbSet
                .Where(r => r.Id == objId)
                .ExecuteDeleteAsync() > 0 ? (true, string.Empty) : (false, "0 Solicitações de registro apagadas."),
            RequestTypeEnum.RETURNS => await DbSet
                .Where(r => (r.Body["BookId"] is Guid ? (Guid)r.Body["BookId"] : Guid.Empty) == objId)
                .ExecuteDeleteAsync() > 0 ? (true, string.Empty) : (false, "0 Solicitações de devolução apagadas."),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public async Task<IEnumerable<Request>> GetRequestsByType(RequestTypeEnum type)
    {
        return await DbSet
            .Where(r => r.Type == type)
            .Include(r => r.User)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Request>> GetActiveRequestsByType(RequestTypeEnum type)
    {
        return await DbSet
            .Where(r => r.Type == type && r.Status == RequestStatusEnum.PENDING)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetActiveRequests()
    {
        return await DbSet
            .Where(r => r.Status == RequestStatusEnum.PENDING)
            .Include(r => r.User)
            .ToListAsync();
    }
}