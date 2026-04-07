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
}