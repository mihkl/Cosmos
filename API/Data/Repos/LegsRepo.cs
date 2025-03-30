using API.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data.Repos;

public class LegsRepo(DataContext dataContext)
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<Leg?> GetLegByIdAsync(Guid legId)
    {
        return await _dataContext.Legs
            .Include(l => l.Providers)
                .ThenInclude(p => p.Company)
            .Include(l => l.RouteInfo)
                .ThenInclude(r => r.From)
            .Include(l => l.RouteInfo)
                .ThenInclude(r => r.To)
            .FirstOrDefaultAsync(l => l.Id == legId);
    }
}
