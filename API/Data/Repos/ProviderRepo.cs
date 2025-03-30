using API.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data.Repos;

public class ProviderRepo(DataContext dataContext)
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<Provider?> GetProviderByIdAsync(Guid providerId)
    {
        return await _dataContext.Providers
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == providerId);
    }
}
