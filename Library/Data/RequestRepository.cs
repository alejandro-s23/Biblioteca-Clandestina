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

    public async Task<IEnumerable<Request>> GetRequestsByType(RequestTypeEnum type)
    {
        return await DbSet.Where(r => r.Type == type).ToListAsync();
    }
    
    public async Task<IEnumerable<Request>> GetActiveRequestsByType(RequestTypeEnum type)
    {
        return await DbSet.Where(r => r.Type == type && r.Status == RequestStatusEnum.PENDING).ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetActiveRequests()
    {
        return await DbSet.Where(r => r.Status == RequestStatusEnum.PENDING).ToListAsync();
    }
}